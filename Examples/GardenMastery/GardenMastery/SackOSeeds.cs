using SR_PluginLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace GardenMastery
{
    class SackOSeeds : MonoBehaviour
    {
        private LandPlot Plot = null;
        private PlotID pid = null;
        public PlotID ID { get { return this.pid; } }
        private Plot_Upgrade_Data data = null;
        private PlotUpgrade upgrade;
        private GardenCatcher _garden_catcher = null;
        private GardenCatcher gardenCatcher { get { if (_garden_catcher == null && Plot != null) { _garden_catcher = Plot.gameObject.GetComponentInChildren<GardenCatcher>(); } return _garden_catcher; } }
        public static List<SackOSeeds> ALL = new List<SackOSeeds>();
        private ModelData mData = null;
        
        public Identifiable.Id Seed { get; private set; }

        private void Awake() { SackOSeeds.ALL.Add(this); }
        private void Destroy() { SackOSeeds.ALL.Remove(this); }

        public void Set_Plot(LandPlot p, PlotUpgrade up)
        {
            this.Plot = p;
            this.pid = new PlotID(Plot);
            this.upgrade = up;

            this.data = new Plot_Upgrade_Data(pid, upgrade.ID);
        }

        public void Set_Seed(Identifiable.Id ID)
        {
            this.Seed = ID;
            data.Set_Int("seed_type", (int)this.Seed);

            if (ID == Identifiable.Id.NONE)
            {
                this.mData.Switch_State(0);
                var paper = mData.Get_Mesh("Sign_Post");
                if (paper == null) throw new Exception("Cannot find model group: \"Sign_Post\"!");
                Image img = paper.GetComponentInChildren<Image>();
                img.gameObject.SetActive(false);
            }
            else
            {
                this.mData.Switch_State(1);
                this.Update_Icon();
            }
        }

        private void Update_Icon()
        {
            var paper = mData.Get_Mesh("Sign_Post");
            if (paper == null) throw new Exception("Cannot find model group: \"Sign_Post\"!");
            Image img = paper.GetComponentInChildren<Image>();
            var sprite = Directors.lookupDirector.GetIcon(this.Seed);
            if (sprite != null)
            {
                img.sprite = sprite;
                img.gameObject.SetActive(true);
            }
            else img.gameObject.SetActive(false);
        }

        public bool IsAccepted(Identifiable.Id id)
        {
            //return (Ident.IsVeggie(id) || Ident.IsFruit(id));
            return gardenCatcher.plantPrefabs.ContainsKey(id);
        }

        private void Reparent_Sign_Post()
        {
            // We want to manually attach our sign post to the STATE_1 sack model so that when it is hidden the sign will also be hidden.
            // Normally we might have just mergeg the meshes in Blender but the sub surf modifier would screw the sign up in this case so we wont.
            var sign = mData.Get_Mesh("Sign_Post");
            if (sign == null) throw new Exception("Cannot find model group: \"Sign_Post\"!");
            var state_1 = mData.Get_State(1);
            if (state_1 == null) throw new Exception("Cannot find model STATE: \"STATE_1\"!");

            sign.transform.SetParent(state_1.transform, true);// We pass true here to be sure the sign doesn't get moved at all.
        }

        private void Reparent_Trigger_Area()
        {
            // We want to manually attach our trigger area to the STATE_0 sack model so that when it is disabled the trigger will be too.
            var trigger = mData.Get_Phys("Trigger");
            if (trigger == null) throw new Exception("Cannot find model PHYS: \"Trigger\"!");
            var state_0 = mData.Get_State(0);
            if (state_0 == null) throw new Exception("Cannot find model STATE: \"STATE_0\"!");

            trigger.transform.SetParent(state_0.transform, true);// We pass true here to be sure it doesn't get moved at all.
        }

        private void Start()
        {
            mData = base.gameObject.GetComponent<ModelData>();
            //mData.Show_Attachment_Points(true);
            Reparent_Sign_Post();
            Reparent_Trigger_Area();
            this.CreateCanvas();
            
            SackOSeedsCatcher catcher = base.gameObject.AddComponent<SackOSeedsCatcher>();
            catcher.activator = this;

            if (!data.HasValue("seed_type")) Set_Seed(Identifiable.Id.NONE);
            else Set_Seed((Identifiable.Id)Enum.ToObject(typeof(Identifiable.Id), data.Get_Int("seed_type")));
        }
        
        private void Update()
        {
            if (Plot == null) return;

            float expireTime = Plot.GetAttachedDeathTime();
            float time = Directors.timeDirector.WorldTime();
            
            if (this.Seed != Identifiable.Id.NONE && time > expireTime) Plant_Seeds();
        }

        private void Plant_Seeds()
        {
            if (Plot == null || gardenCatcher == null) return;

            GameObject prefab;
            if(!gardenCatcher.plantPrefabs.TryGetValue(Seed, out prefab)) throw new ArgumentNullException(String.Format("Cannot find plant prefab for: {0}", Seed));

            GameObject gameObject = UnityEngine.Object.Instantiate(prefab, this.Plot.transform.position, this.Plot.transform.rotation) as GameObject;
            if (gameObject == null) throw new ArgumentNullException("Cannot create garden resource spawn.");

            gameObject.AddComponent<DestroyAfterTime>();
            this.Plot.Attach(gameObject);
            
            if (gardenCatcher.acceptFX != null)
            {
                SRBehaviour.InstantiateDynamic(gardenCatcher.acceptFX, base.transform.position, base.transform.rotation);
            }
            this.Set_Seed( Identifiable.Id.NONE );
        }

        private void CreateCanvas()
        {
            GameObject paper = mData.Get_Mesh("Sign_Post");
            GameObject newCanvas = base.gameObject;

            Canvas c = newCanvas.AddComponent<Canvas>();
            c.renderMode = RenderMode.WorldSpace;
            var scaler = newCanvas.AddComponent<CanvasScaler>();
            newCanvas.AddComponent<GraphicRaycaster>();

            RectTransform rt = c.GetComponent<RectTransform>();
            
            GameObject panel = new GameObject("icon");
            panel.transform.SetParent(paper.transform, false);
            
            panel.AddComponent<CanvasRenderer>();
            Image img = panel.AddComponent<Image>();
            img.transform.Rotate(Vector3.up, -90f);

            Material mat = new Material(Shader.Find("Transparent/Diffuse"));
            img.material = mat;
            img.SetMaterialDirty();

            const float scale = 0.0044f;
            img.transform.localScale = new Vector3(scale, scale, scale);

            img.transform.localPosition = mData.Get_Attachment_Point("icon").transform.localPosition;
            img.color = Color.white;
        }
    }
}
