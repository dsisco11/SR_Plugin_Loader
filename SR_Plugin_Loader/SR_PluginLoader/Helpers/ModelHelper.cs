using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using ObjLoader.Loader;
using ObjLoader.Loader.Data;
using ObjLoader.Loader.Loaders;
using ObjLoader.Loader.Data.Elements;
using System.Text.RegularExpressions;

namespace SR_PluginLoader
{
    public delegate Stream ModelMat_Resolver_Delegate(string fileName);
    public class MaterialResourceProvider : IMaterialStreamProvider
    {
        private ModelMat_Resolver_Delegate resolver = null;
        public MaterialResourceProvider(ModelMat_Resolver_Delegate cb) { resolver = cb; }

        public Stream Open(string fileName)
        {
            if (resolver == null) return null;
            return resolver(fileName);
        }
    }
    
    public enum Model_Prefab_Transform
    {
        None=0,
        Flip_X,
        Flip_Y,
        Flip_Z,
    }

    /// <summary>
    /// RESERVED PREFIXES:
    /// STATE_# : Removes any suffixes such that the resulting sub-GameObject's name is just the prefix. This allows for easily switching between different model versions.
    /// POS_ : Does not attach any scripts to the resulting sub-GameObject, these objects serve as attachment points on a model.
    /// PHYS_ : Does not render, instead the resulting sub-GameObject merely serves as a bounding box.
    /// </summary>
    public static class ModelHelper
    {
        private enum MdlGroupType
        {
            MESH = 0,
            STATE,
            ATTACHMENT_POS,
            PHYS,
        }

        public static Dictionary<string, ModelData_Header> data_cache = new Dictionary<string, ModelData_Header>();
        private static Dictionary<string, GameObject> model_prefab_cache = new Dictionary<string, GameObject>();
        private static Dictionary<Regex, MdlGroupType> RESERVED_GROUP_PREFIXES = new Dictionary<Regex, MdlGroupType>()
        {
            { new Regex(@"^(STATE_\d+)\w*"), MdlGroupType.STATE },
            { new Regex(@"^PHYS_\d*(\w*)"), MdlGroupType.PHYS },
            { new Regex(@"^POS_\d*(\w*)"), MdlGroupType.ATTACHMENT_POS },
        };

        public static GameObject Get_Prefab(string prefabName)
        {
            if (model_prefab_cache.ContainsKey(prefabName)) return model_prefab_cache[prefabName];

            return null;
        }

        public static GameObject Create_Model_Prefab(string prefabName, Stream obj_file, ModelMat_Resolver_Delegate material_resolver, Model_Prefab_Transform[] transforms=null)
        {
            if (model_prefab_cache.ContainsKey(prefabName)) return model_prefab_cache[prefabName];

            List<Mesh> meshList = new List<Mesh>();
            ObjLoaderFactory factory = new ObjLoaderFactory();
            IObjLoader loader;

            if (material_resolver != null) loader = factory.Create(new MaterialResourceProvider(material_resolver));
            else loader = factory.Create();

            var mdl = loader.Load(obj_file);

            MaterialBuilder matBuilder = new MaterialBuilder(material_resolver);

            GameObject prefab = new GameObject(prefabName);
            prefab.SetActive(false);
            prefab.AddComponent<ModelData>();
            prefab.AddComponent<Rigidbody>();
            ModelData_Header data = new ModelData_Header();
            data_cache.Add(prefabName, data);


            foreach (var group in mdl.Groups)
            {
                MdlGroupType gType = MdlGroupType.MESH;
                string Name = group.Name;

                foreach (KeyValuePair<Regex, MdlGroupType> kvp in RESERVED_GROUP_PREFIXES)
                {
                    Regex reg = kvp.Key;
                    Match match = reg.Match(Name);
                    if (!match.Success) continue;
                    if (match.Groups.Count <= 1) continue;

                    //for (int i = 0; i < match.Groups.Count; i++) DebugHud.Log("  - {0}", match.Groups[i].Value);

                    Name = match.Groups[1].Value.TrimStart(new char[] { '_' });
                    gType = kvp.Value;
                    break;
                }

                //DebugHud.Log("- Group: \"{0}\" Type: {1}", gName, Enum.GetName(typeof(MdlGroupType), gType));
                
                GameObject gObj = new GameObject(group.Name);
                MeshBuilder builder = new MeshBuilder(mdl);
                List<UnityEngine.Material> materials = new List<UnityEngine.Material>();

                foreach (SubMesh mesh in group.Meshes)
                {
                    builder.Start_Sub_Mesh();
                    foreach (var face in mesh.Faces)
                    {
                        if (face.Count < 3) continue;
                        else if (face.Count == 3)
                        {
                            builder.Push_Tri(face[0], face[1], face[2], transforms);
                        }
                        else if (face.Count == 4)
                        {
                            builder.Push_Tri(face[0], face[1], face[2], transforms);
                            builder.Push_Tri(face[0], face[2], face[3], transforms);
                        }
                    }

                    if (gType != MdlGroupType.ATTACHMENT_POS && gType != MdlGroupType.PHYS)// Attachment points don't render(normally)
                    {
                        if (mesh.Material != null)
                        {
                            //DebugHud.Log("-   Material: {0}", mesh.Material.Name);
                            materials.Add(matBuilder.Create(mesh.Material));
                        }
                    }
                }
                gObj.transform.SetParent(prefab.transform, false);
                gObj.transform.localPosition = Vector3.zero;
                gObj.transform.localRotation = Quaternion.identity;


                if (gType == MdlGroupType.ATTACHMENT_POS)
                {
                    // Attachment positions should ideally contain only a single vertex. But if they contain more we should not be stupid and ignore them.
                    // So let's just find the center of all points provided.
                    gObj.transform.localPosition = builder.Calculate_Center_Point(group);
                    data.Push_Attachment_Spot(group.Name, Name);
                }
                else
                {
                    MeshFilter filter = gObj.AddComponent<MeshFilter>();
                    filter.mesh = builder.Compile();

                    MeshRenderer renderer = gObj.AddComponent<MeshRenderer>();// All game objects need a MeshRenderer for that mesh to even give the object a size/bounds within the world!
                    if (gType == MdlGroupType.PHYS || materials.Count <= 0)
                    {
                        renderer.enabled = false;
                    }
                    else
                    {
                        //DebugHud.Log("Creating material for: {0}", group.Material.Name);
                        renderer.enabled = true;
                        renderer.materials = materials.ToArray();
                    }

                    var bx = gObj.AddComponent<BoxCollider>();

                    switch(gType)
                    {
                        case MdlGroupType.MESH:
                            data.Push_Mesh(group.Name, Name);
                            break;
                        case MdlGroupType.STATE:
                            data.Push_State(group.Name, Name);
                            break;
                        case MdlGroupType.PHYS:
                            data.Push_Phys(group.Name, Name);
                            break;
                        default:
                            throw new NotImplementedException("Unhandled "+ nameof(MdlGroupType) +" value: "+ gType);
                    }
                }
            }
            
            if (model_prefab_cache.ContainsKey(prefabName)) model_prefab_cache.Add(prefabName, prefab);
            else model_prefab_cache[prefabName] = prefab;

            return prefab;
        }
    }

    public class MeshBuilder
    {
        private LoadResult Model = null;
        private int _submesh_idx = -1;

        private List<Vector3> verts = new List<Vector3>();
        private List<Vector3> normals = new List<Vector3>();
        private List<Vector2> uvw = new List<Vector2>();
        private List<Color> colors = new List<Color>();

        private List<List<int>> tri_list = new List<List<int>>();

        public MeshBuilder(LoadResult mdl) { Model = mdl; }

        protected Vector3 toVector(ObjLoader.Loader.Data.VertexData.Vertex v) { return new Vector3(v.X, v.Y, v.Z); }
        protected Vector3 toVector(ObjLoader.Loader.Data.VertexData.Normal v) { return new Vector3(v.X, v.Y, v.Z); }
        protected Vector2 toVector(ObjLoader.Loader.Data.VertexData.Texture v) { return new Vector2(v.X, v.Y); }

        public Mesh Compile()
        {
            Mesh mesh = new Mesh();

            mesh.vertices = verts.ToArray();
            if (normals.Count == verts.Count) mesh.normals = normals.ToArray();
            //else if (normals.Count > 0) DebugHud.Log("Number of normals doesn't match the number of verticies!");

            if (uvw.Count == verts.Count) mesh.uv = uvw.ToArray();
            //else if (uvw.Count > 0) DebugHud.Log("Number of texture coords doesn't match the number of verticies!");

            if (colors.Count == verts.Count) mesh.colors = colors.ToArray();
            //else if (colors.Count > 0) DebugHud.Log("Number of vertex colors doesn't match the number of verticies!");

            mesh.subMeshCount = tri_list.Count;
            for(int i=0; i<tri_list.Count; i++)
            {
                List<int> tris = tri_list[i];
                mesh.SetTriangles(tris.ToArray(), i);
            }

            mesh.Optimize();
            mesh.RecalculateBounds();
            mesh.UploadMeshData(true);
            return mesh;
        }

        /// <summary>
        /// Packages all of the currently pushed triangles into a tri-list which will makeup a submesh within the Unity Mesh object.
        /// </summary>
        public void Start_Sub_Mesh()
        {
            tri_list.Add( new List<int>() );
            _submesh_idx += 1;
        }

        public void Push_Indice(int idx) { tri_list[_submesh_idx].Add(idx); }
        
        public void Push_Vert(Vector3 vert) { verts.Add(vert); }

        public void Push_UV(Vector2 uv) { uvw.Add(uv); }

        public void Push_Normal(Vector3 norm) { normals.Add(norm); }

        public void Push_Color(Color clr) { colors.Add(clr); }

        public void Push(FaceVertex face, Model_Prefab_Transform[] transforms)
        {
            int vIdx = face.VertexIndex-1;
            int nIdx = face.NormalIndex-1;
            int tIdx = face.TextureIndex-1;
            //DebugHud.Log("vIdx<{0}>  nIdx<{1}>  tIdx<{2}>", vIdx, nIdx, tIdx);
            Vector3 scaleVec = Vector3.one;

            if (Model.Vertices.Count > vIdx && vIdx > -1)
            {
                var v = Model.Vertices[vIdx];
                float sX = 1f, sY = 1f, sZ = 1f;

                if (transforms != null)
                {
                    foreach (Model_Prefab_Transform trans in transforms)
                    {
                        switch (trans)
                        {
                            case Model_Prefab_Transform.Flip_X:
                                sX = -1f;
                                break;
                            case Model_Prefab_Transform.Flip_Y:
                                sY = -1f;
                                break;
                            case Model_Prefab_Transform.Flip_Z:
                                sZ = -1f;
                                break;
                            default:
                                throw new NotImplementedException(String.Format("Model_Prefab_Transform: {0} is not yet implemented!", Enum.GetName(typeof(Model_Prefab_Transform), trans)));
                                break;
                        }
                    }
                }

                scaleVec = new Vector3(sX, sY, sZ);
                Vector3 vert = new Vector3(v.X, v.Y, v.Z);
                vert.Scale(scaleVec);

                int vi = verts.Count;
                Push_Vert(vert);
                Push_Indice(vi);
            }
            
            if (Model.Normals.Count > nIdx && nIdx > -1)
            {
                var n = Model.Normals[nIdx];
                Vector3 normal = new Vector3(n.X, n.Y, n.Z);
                normal.Scale(scaleVec);
                Push_Normal(normal);
            }

            if (Model.Textures.Count > tIdx && tIdx > -1)
            {
                var u = Model.Textures[tIdx];
                Push_UV(new Vector2(u.X, u.Y));
            }
        }

        public void Push_Tri(FaceVertex A, FaceVertex B, FaceVertex C, Model_Prefab_Transform[] transforms)
        {
            if(transforms != null)
            {
                foreach(Model_Prefab_Transform trans in transforms)
                {
                    switch(trans)
                    {
                        case Model_Prefab_Transform.Flip_X:
                        case Model_Prefab_Transform.Flip_Y:
                        case Model_Prefab_Transform.Flip_Z:
                            FaceVertex vA = A, vB = B, vC = C;
                            A = vA;
                            B = vC;
                            C = vB;
                            break;
                        default:
                            throw new NotImplementedException(String.Format("Model_Prefab_Transform: {0} is not yet implemented!", Enum.GetName(typeof(Model_Prefab_Transform), trans)));
                            break;
                    }
                }
            }

            this.Push(A, transforms);
            this.Push(B, transforms);
            this.Push(C, transforms);
        }

        public Vector3 Calculate_Center_Point(ObjLoader.Loader.Data.Elements.Group grp)
        {
            V3 min = null;
            V3 max = null;
            // Find the bounds
            foreach (ObjLoader.Loader.Data.VertexData.Vertex vec in grp.Verticies)
            {
                V3 v = new V3(vec);

                if (min==null) min = v;
                if (max==null) max = v;

                if (v.x < min.x) min.x = v.x;
                if (v.y < min.y) min.y = v.y;
                if (v.z < min.z) min.z = v.z;

                if (v.x > max.x) max.x = v.x;
                if (v.y > max.y) max.y = v.y;
                if (v.z > max.z) max.z = v.z;
            }

            if (min == null || max == null) return Vector3.zero;

            // Find the center
            V3 delta = new V3((max.x-min.x), (max.y-min.y), (max.z-min.z));
            float X, Y, Z;
            X = min.x + (delta.x * 0.5f);
            Y = min.y + (delta.y * 0.5f);
            Z = min.z + (delta.z * 0.5f);
            return new Vector3(X, Y, Z);
        }
    }

    internal class V3
    {
        public float x, y, z;
        public V3() { x = y = z = 0f; }
        public V3(Vec3 v) { x = v.X; y = v.Y; z = v.Z; }
        public V3(ObjLoader.Loader.Data.VertexData.Vertex v) { x = v.X; y = v.Y; z = v.Z; }
        public V3(Vector3 v) { x = v.x; y = v.y; z = v.z; }
        public V3(float vx, float vy, float vz) { x = vx; y = vy; z = vz; }

        public override string ToString()
        {
            return String.Format("<{0:0.0}, {1:0.0}, {2:0.0}>", x, y, z);
        }
    }

    public class MaterialBuilder
    {
        ModelMat_Resolver_Delegate resolver = null;
        public MaterialBuilder(ModelMat_Resolver_Delegate dgt) { resolver = dgt; }

        public UnityEngine.Material Create(ObjLoader.Loader.Data.Material material)
        {
            bool HasDiffuse = ValidStr(material.DiffuseTextureMap);
            bool HasBump = ValidStr(material.BumpMap);
            bool HasParallax = ValidStr(material.DisplacementMap);
            bool HasSpecular_Color = ValidStr(material.SpecularTextureMap);// Specular color I think
            bool HasSpecular_Hardness = ValidStr(material.SpecularHighlightTextureMap);// Specular INTENSITY map
            bool HasEmission = ValidStr(material.EmissiveTextureMap);
            bool HasAmbientOcclusion = ValidStr(material.AmbientTextureMap);
            bool HasTransparency = ValidStr(material.AlphaTextureMap);
            bool HasDecals = ValidStr(material.StencilDecalMap);// Ehh, Someone else will implement support for decals eventually... yeah right...

            // Here we will just go ahead and set any of these flags TRUE if we can already tell they are going to be needed. 
            // This ensures we don't get bad materials when bad moeling programs don't write the correct IlluminationMode into the .mtl files.

            bool _specularity = (HasSpecular_Color || HasSpecular_Hardness);//does the shader we are using support specularity parameters
            bool _lighting = HasBump;// Does the material receive lighting from the environment?
            bool _transparency = HasTransparency;// Does this material need to allow use of the alpha channel when drawing textures?
            bool _reflection = false;
            bool _refraction = false;
            bool _shadows = false;// Does this material cast shadows?
            

            // First let's instantiate a new Unity material using the appropriate shader.
            UnityEngine.Material mat = null;
            if (material.IlluminationModel >= 1) _lighting = true;
            if (material.IlluminationModel >= 2) _specularity = true;
            if (material.IlluminationModel >= 3) _reflection = true;
            if (material.IlluminationModel >= 4) _transparency = true;


            switch (material.IlluminationModel)
            {
                case 0:// Diffuse
                    break;
                case 1:// Diffuse + Ambient
                    break;
                case 2:// Diffuse + Ambient + Specularity
                    break;
                case 3:// Reflection on and Ray trace on
                    _reflection = true;
                    break;
                case 4:// Transparency: Glass on, Reflection: Ray trace on
                    _reflection = true;
                    break;
                case 6:// Transparency: Refraction on, Reflection: Fresnel off and Ray trace on
                    _reflection = true;
                    break;
                case 7:// Transparency: Refraction on, Reflection: Fresnel on and Ray trace on
                    _reflection = true;
                    _refraction = true;
                    break;
                case 9:// Transparency: Glass on, Reflection: Ray trace off
                    break;
                case 10:// Casts shadows onto invisible surfaces
                    break;
            }

            // Instantiate a material
            if (mat == null && _refraction && _transparency) { }// Fallthrough to the transparent/diffuse shader for now as we presently have no refraction capable shaders.
            if (mat == null && _transparency) mat = new UnityEngine.Material(Shader.Find("Transparent/Diffuse"));

            //if (mat == null && _lighting) mat = new UnityEngine.Material(Shader.Find("Diffuse"));
            if (mat == null && !_lighting) mat = new UnityEngine.Material(Shader.Find("VertexLit"));
            if (mat == null) mat = new UnityEngine.Material(Shader.Find("Standard"));
            // Name it
            mat.name = material.Name;

            // Set any necessarry shader params
            Color albedo = toUnityColor(material.DiffuseColor);
            albedo.a = material.Transparency;
            mat.SetColor("_Color", albedo);

            if (_lighting) mat.SetColor("_EmissionColor", toUnityColor(material.AmbientColor));
            if (_specularity) mat.SetColor("_SpecColor", toUnityColor(material.SpecularColor));
            //mat.SetFloat("_Glossiness", );// We might actually want this to be set to 1.0 because .mlt specs don't define a specularIntensity. instead the specular color is multiplied by the intensity before modeling programs export the material file. so intensity is already encoded in the color...

            // Now load all of the textures our material needs...
            if (HasDiffuse) this.ApplyTexture(mat, "_MainTex", material.DiffuseTextureMap);// mat.SetTexture("_MainTex", LoadTexture(material.DiffuseTextureMap)); //mat.mainTexture = LoadTexture(material.DiffuseTextureMap);
            if (HasBump) this.ApplyTexture(mat, "_BumpMap", material.BumpMap);// mat.SetTexture("_BumpMap", LoadTexture(material.BumpMap));
            if (HasParallax) this.ApplyTexture(mat, "_ParallaxMap", material.DisplacementMap);// mat.SetTexture("_ParallaxMap", LoadTexture(material.DisplacementMap));

            //if (HasSpecular_Color) this.ApplyTexture(mat, "", material.SpecularTextureMap);// mat.SetTexture("", LoadTexture(material.SpecularTextureMap));
            if (HasSpecular_Hardness) this.ApplyTexture(mat, "_MetallicGlossMap", material.SpecularHighlightTextureMap);// mat.SetTexture("_MetallicGlossMap", LoadTexture(material.SpecularHighlightTextureMap));
            if (HasAmbientOcclusion) this.ApplyTexture(mat, "_OcclusionMap", material.AmbientTextureMap);// mat.SetTexture("_OcclusionMap", LoadTexture(material.AmbientTextureMap));
            if (HasEmission) this.ApplyTexture(mat, "_EmissionMap", material.EmissiveTextureMap);// mat.SetTexture("_EmissionMap", LoadTexture(material.EmissiveTextureMap));

            return mat;
        }

        private Color toUnityColor(Vec3 c) { return new Color(c.X, c.Y, c.Z, 1f); }
        private bool ValidStr(string str) { return (str != null && str.Length > 0); }

        private Stream Resolve(string file)
        {
            if (resolver == null) throw new ArgumentNullException(String.Format("Material resolver callback is null, Cannot locate texture: {0}", file));

            //DebugHud.LogSilent("[MaterialBuilder] Resolving file: {0}", file);
            var stream = resolver(file);
            if (stream == null) DebugHud.Log("Cannot resolve file: {0}", file);

            return stream;
        }

        private Texture LoadTexture(string file, out TextureHelper.TextureType texType)
        {
            texType = TextureHelper.TextureType.UNKNOWN;
            Stream strm = Resolve(file);
            if (strm == null) return null;

            byte[] data = Util.Read_Stream(strm);
            strm.Close();

            texType = TextureHelper.Identify_Texture_Type(data);
            Texture tex = TextureHelper.Load(data);
            tex.name = file;
            return tex;
        }

        private void ApplyTexture(UnityEngine.Material mat, string propertyName, string textureFile)
        {
            TextureHelper.TextureType texType;
            Texture tex = LoadTexture(textureFile, out texType);
            if (tex == null) return;

            mat.SetTexture(propertyName, tex);

            // Unity is stupid and flips DXT textures upside down (or more likely fails to flip them right side up). so we need to do it ourselves...
            if (texType == TextureHelper.TextureType.DXT) mat.SetTextureScale(propertyName, new Vector2(1f, -1f));
        }
    }
}
