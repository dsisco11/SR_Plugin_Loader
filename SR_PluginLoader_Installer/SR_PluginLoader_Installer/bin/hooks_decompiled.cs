// Targeted Assembly DLL: c:\program files (x86)\steam\SteamApps\common\Slime Rancher\SlimeRancher_Data\Managed\Assembly-CSharp.dll


/*
IL_0000: ldnull
IL_0001: stloc V_12
IL_0005: nop
IL_0006: ldarg.0
IL_0007: ldfld System.Boolean CellDirector::allowSpawns
IL_000c: brtrue IL_0012
IL_0011: ret
IL_0012: nop
IL_0013: call System.Single UnityEngine.Time::get_time()
IL_0018: ldarg.0
IL_0019: ldfld System.Single CellDirector::spawnThrottleTime
IL_001e: blt.un IL_0370
IL_0023: ldarg.0
IL_0024: ldfld TimeDirector CellDirector::timeDir
IL_0029: ldarg.0
IL_002a: ldfld System.Single CellDirector::nextSpawn
IL_002f: callvirt System.Boolean TimeDirector::HasReached(System.Single)
IL_0034: brfalse IL_02cc
IL_0039: ldarg.0
IL_003a: ldfld SECTR_Sector CellDirector::sector
IL_003f: callvirt System.Boolean SECTR_Member::get_Hibernate()
IL_0044: brtrue IL_02f4
IL_0049: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::Pre_Region_Spawn_Cycle
IL_004e: ldarg 
IL_0052: ldloca V_12
IL_0056: ldnull
IL_0057: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_005c: stloc V_13
IL_0060: ldloc V_13
IL_0064: ldfld System.Boolean SR_PluginLoader._hook_result::abort
IL_0069: brfalse IL_006f
IL_006e: ret
IL_006f: nop
IL_0070: ldarg.0
IL_0071: ldfld System.Collections.Generic.List`1<DirectedSlimeSpawner> CellDirector::spawners
IL_0076: callvirt System.Int32 System.Collections.Generic.List`1<DirectedSlimeSpawner>::get_Count()
IL_007b: ldc.i4.0
IL_007c: ble IL_0178
IL_0081: ldarg.0
IL_0082: call System.Boolean CellDirector::NeedsMoreSlimes()
IL_0087: brfalse IL_0179
IL_008c: ldarg.0
IL_008d: call System.Boolean CellDirector::HasTarrSlimes()
IL_0092: brtrue IL_017a
IL_0097: newobj System.Void System.Collections.Generic.Dictionary`2<DirectedSlimeSpawner,System.Single>::.ctor()
IL_009c: stloc.0
IL_009d: ldc.r4 0
IL_00a2: stloc.1
IL_00a3: ldarg.0
IL_00a4: ldfld System.Collections.Generic.List`1<DirectedSlimeSpawner> CellDirector::spawners
IL_00a9: callvirt System.Collections.Generic.List`1/Enumerator<!0> System.Collections.Generic.List`1<DirectedSlimeSpawner>::GetEnumerator()
IL_00ae: stloc.3
IL_00af: br IL_00e9
IL_00b4: nop
IL_00b5: ldloca.s V_3
IL_00b7: call !0 System.Collections.Generic.List`1/Enumerator<DirectedSlimeSpawner>::get_Current()
IL_00bc: stloc.2
IL_00bd: ldloc.2
IL_00be: ldloca.s V_10
IL_00c0: initobj System.Nullable`1<System.Single>
IL_00c6: ldloc.s V_10
IL_00c8: callvirt System.Boolean DirectedActorSpawner::CanSpawn(System.Nullable`1<System.Single>)
IL_00cd: brfalse IL_00e8
IL_00d2: ldloc.0
IL_00d3: ldloc.2
IL_00d4: ldloc.2
IL_00d5: ldfld System.Single DirectedActorSpawner::directedSpawnWeight
IL_00da: callvirt System.Void System.Collections.Generic.Dictionary`2<DirectedSlimeSpawner,System.Single>::set_Item(!0,!1)
IL_00df: ldloc.1
IL_00e0: ldloc.2
IL_00e1: ldfld System.Single DirectedActorSpawner::directedSpawnWeight
IL_00e6: add
IL_00e7: stloc.1
IL_00e8: nop
IL_00e9: ldloca.s V_3
IL_00eb: call System.Boolean System.Collections.Generic.List`1/Enumerator<DirectedSlimeSpawner>::MoveNext()
IL_00f0: brtrue IL_00b4
IL_00f5: leave IL_0106
IL_00fa: ldloc.3
IL_00fb: box System.Collections.Generic.List`1/Enumerator<DirectedSlimeSpawner>
IL_0100: callvirt System.Void System.IDisposable::Dispose()
IL_0105: endfinally
IL_0106: ldloc.0
IL_0107: callvirt System.Int32 System.Collections.Generic.Dictionary`2<DirectedSlimeSpawner,System.Single>::get_Count()
IL_010c: ldc.i4.0
IL_010d: ble IL_0173
IL_0112: ldloc.1
IL_0113: ldc.r4 0
IL_0118: ble.un IL_0173
IL_011d: ldarg.0
IL_011e: ldfld Randoms CellDirector::rand
IL_0123: ldloc.0
IL_0124: ldnull
IL_0125: callvirt !!0 Randoms::Pick<DirectedSlimeSpawner>(System.Collections.Generic.IDictionary`2<!!0,System.Single>,!!0)
IL_012a: stloc.s V_4
IL_012c: call T SRSingleton`1<SceneContext>::get_Instance()
IL_0131: callvirt ModDirector SceneContext::get_ModDirector()
IL_0136: callvirt System.Single ModDirector::SlimeCountFactor()
IL_013b: stloc.s V_5
IL_013d: ldarg.0
IL_013e: ldloc.s V_4
IL_0140: ldarg.0
IL_0141: ldfld Randoms CellDirector::rand
IL_0146: ldarg.0
IL_0147: ldfld System.Int32 CellDirector::minPerSpawn
IL_014c: ldarg.0
IL_014d: ldfld System.Int32 CellDirector::maxPerSpawn
IL_0152: ldc.i4.1
IL_0153: add
IL_0154: callvirt System.Int32 Randoms::GetInRange(System.Int32,System.Int32)
IL_0159: conv.r4
IL_015a: ldloc.s V_5
IL_015c: mul
IL_015d: call System.Int32 UnityEngine.Mathf::RoundToInt(System.Single)
IL_0162: ldarg.0
IL_0163: ldfld Randoms CellDirector::rand
IL_0168: callvirt System.Collections.IEnumerator DirectedActorSpawner::Spawn(System.Int32,Randoms)
IL_016d: call UnityEngine.Coroutine UnityEngine.MonoBehaviour::StartCoroutine(System.Collections.IEnumerator)
IL_0172: pop
IL_0173: br IL_01a5
IL_0178: nop
IL_0179: nop
IL_017a: nop
IL_017b: ldarg.0
IL_017c: call System.Boolean CellDirector::HasTooManySlimes()
IL_0181: brfalse IL_01a4
IL_0186: ldarg.0
IL_0187: ldarg.0
IL_0188: ldfld System.Collections.Generic.List`1<UnityEngine.GameObject> CellDirector::slimes
IL_018d: ldc.r4 0.1
IL_0192: ldarg.0
IL_0193: ldfld System.Int32 CellDirector::targetSlimeCount
IL_0198: conv.r4
IL_0199: mul
IL_019a: call System.Int32 UnityEngine.Mathf::CeilToInt(System.Single)
IL_019f: call System.Void CellDirector::Despawn(System.Collections.Generic.List`1<UnityEngine.GameObject>,System.Int32)
IL_01a4: nop
IL_01a5: ldarg.0
IL_01a6: ldfld System.Collections.Generic.List`1<DirectedAnimalSpawner> CellDirector::animalSpawners
IL_01ab: callvirt System.Int32 System.Collections.Generic.List`1<DirectedAnimalSpawner>::get_Count()
IL_01b0: ldc.i4.0
IL_01b1: ble IL_0271
IL_01b6: ldarg.0
IL_01b7: call System.Boolean CellDirector::NeedsMoreAnimals()
IL_01bc: brfalse IL_0272
IL_01c1: newobj System.Void System.Collections.Generic.List`1<DirectedAnimalSpawner>::.ctor()
IL_01c6: stloc.s V_6
IL_01c8: ldarg.0
IL_01c9: ldfld System.Collections.Generic.List`1<DirectedAnimalSpawner> CellDirector::animalSpawners
IL_01ce: callvirt System.Collections.Generic.List`1/Enumerator<!0> System.Collections.Generic.List`1<DirectedAnimalSpawner>::GetEnumerator()
IL_01d3: stloc.s V_8
IL_01d5: br IL_0204
IL_01da: nop
IL_01db: ldloca.s V_8
IL_01dd: call !0 System.Collections.Generic.List`1/Enumerator<DirectedAnimalSpawner>::get_Current()
IL_01e2: stloc.s V_7
IL_01e4: ldloc.s V_7
IL_01e6: ldloca.s V_11
IL_01e8: initobj System.Nullable`1<System.Single>
IL_01ee: ldloc.s V_11
IL_01f0: callvirt System.Boolean DirectedAnimalSpawner::CanSpawn(System.Nullable`1<System.Single>)
IL_01f5: brfalse IL_0203
IL_01fa: ldloc.s V_6
IL_01fc: ldloc.s V_7
IL_01fe: callvirt System.Void System.Collections.Generic.List`1<DirectedAnimalSpawner>::Add(!0)
IL_0203: nop
IL_0204: ldloca.s V_8
IL_0206: call System.Boolean System.Collections.Generic.List`1/Enumerator<DirectedAnimalSpawner>::MoveNext()
IL_020b: brtrue IL_01da
IL_0210: leave IL_0222
IL_0215: ldloc.s V_8
IL_0217: box System.Collections.Generic.List`1/Enumerator<DirectedAnimalSpawner>
IL_021c: callvirt System.Void System.IDisposable::Dispose()
IL_0221: endfinally
IL_0222: ldloc.s V_6
IL_0224: callvirt System.Int32 System.Collections.Generic.List`1<DirectedAnimalSpawner>::get_Count()
IL_0229: ldc.i4.0
IL_022a: ble IL_026c
IL_022f: ldarg.0
IL_0230: ldfld Randoms CellDirector::rand
IL_0235: ldloc.s V_6
IL_0237: ldnull
IL_0238: callvirt !!0 Randoms::Pick<DirectedAnimalSpawner>(System.Collections.Generic.ICollection`1<!!0>,!!0)
IL_023d: stloc.s V_9
IL_023f: ldarg.0
IL_0240: ldloc.s V_9
IL_0242: ldarg.0
IL_0243: ldfld Randoms CellDirector::rand
IL_0248: ldarg.0
IL_0249: ldfld System.Int32 CellDirector::minAnimalPerSpawn
IL_024e: ldarg.0
IL_024f: ldfld System.Int32 CellDirector::maxAnimalPerSpawn
IL_0254: ldc.i4.1
IL_0255: add
IL_0256: callvirt System.Int32 Randoms::GetInRange(System.Int32,System.Int32)
IL_025b: ldarg.0
IL_025c: ldfld Randoms CellDirector::rand
IL_0261: callvirt System.Collections.IEnumerator DirectedAnimalSpawner::Spawn(System.Int32,Randoms)
IL_0266: call UnityEngine.Coroutine UnityEngine.MonoBehaviour::StartCoroutine(System.Collections.IEnumerator)
IL_026b: pop
IL_026c: br IL_029d
IL_0271: nop
IL_0272: nop
IL_0273: ldarg.0
IL_0274: call System.Boolean CellDirector::HasTooManyAnimals()
IL_0279: brfalse IL_029c
IL_027e: ldarg.0
IL_027f: ldarg.0
IL_0280: ldfld System.Collections.Generic.List`1<UnityEngine.GameObject> CellDirector::animals
IL_0285: ldc.r4 0.1
IL_028a: ldarg.0
IL_028b: ldfld System.Int32 CellDirector::targetAnimalCount
IL_0290: conv.r4
IL_0291: mul
IL_0292: call System.Int32 UnityEngine.Mathf::CeilToInt(System.Single)
IL_0297: call System.Void CellDirector::Despawn(System.Collections.Generic.List`1<UnityEngine.GameObject>,System.Int32)
IL_029c: nop
IL_029d: ldarg.0
IL_029e: dup
IL_029f: ldfld System.Single CellDirector::nextSpawn
IL_02a4: ldarg.0
IL_02a5: ldfld System.Single CellDirector::avgSpawnTimeGameHours
IL_02aa: ldc.r4 3600
IL_02af: mul
IL_02b0: ldarg.0
IL_02b1: ldfld Randoms CellDirector::rand
IL_02b6: ldc.r4 0.5
IL_02bb: ldc.r4 1.5
IL_02c0: callvirt System.Single Randoms::GetInRange(System.Single,System.Single)
IL_02c5: mul
IL_02c6: add
IL_02c7: stfld System.Single CellDirector::nextSpawn
IL_02cc: nop
IL_02cd: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::Post_Region_Spawn_Cycle
IL_02d2: ldarg 
IL_02d6: ldloca V_12
IL_02da: ldnull
IL_02db: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_02e0: stloc V_14
IL_02e4: ldloc V_14
IL_02e8: ldfld System.Boolean SR_PluginLoader._hook_result::abort
IL_02ed: brfalse IL_02f3
IL_02f2: ret
IL_02f3: nop
IL_02f4: nop
IL_02f5: ldarg.0
IL_02f6: ldfld System.Collections.Generic.List`1<UnityEngine.GameObject> CellDirector::slimes
IL_02fb: callvirt System.Int32 System.Collections.Generic.List`1<UnityEngine.GameObject>::get_Count()
IL_0300: ldarg.0
IL_0301: ldfld System.Int32 CellDirector::cullSlimesLimit
IL_0306: ble IL_0329
IL_030b: ldarg.0
IL_030c: ldarg.0
IL_030d: ldfld System.Collections.Generic.List`1<UnityEngine.GameObject> CellDirector::slimes
IL_0312: ldarg.0
IL_0313: ldfld System.Collections.Generic.List`1<UnityEngine.GameObject> CellDirector::slimes
IL_0318: callvirt System.Int32 System.Collections.Generic.List`1<UnityEngine.GameObject>::get_Count()
IL_031d: ldarg.0
IL_031e: ldfld System.Int32 CellDirector::cullSlimesLimit
IL_0323: sub
IL_0324: call System.Void CellDirector::Despawn(System.Collections.Generic.List`1<UnityEngine.GameObject>,System.Int32)
IL_0329: nop
IL_032a: ldarg.0
IL_032b: ldfld System.Collections.Generic.List`1<UnityEngine.GameObject> CellDirector::animals
IL_0330: callvirt System.Int32 System.Collections.Generic.List`1<UnityEngine.GameObject>::get_Count()
IL_0335: ldarg.0
IL_0336: ldfld System.Int32 CellDirector::cullAnimalsLimit
IL_033b: ble IL_035e
IL_0340: ldarg.0
IL_0341: ldarg.0
IL_0342: ldfld System.Collections.Generic.List`1<UnityEngine.GameObject> CellDirector::animals
IL_0347: ldarg.0
IL_0348: ldfld System.Collections.Generic.List`1<UnityEngine.GameObject> CellDirector::animals
IL_034d: callvirt System.Int32 System.Collections.Generic.List`1<UnityEngine.GameObject>::get_Count()
IL_0352: ldarg.0
IL_0353: ldfld System.Int32 CellDirector::cullAnimalsLimit
IL_0358: sub
IL_0359: call System.Void CellDirector::Despawn(System.Collections.Generic.List`1<UnityEngine.GameObject>,System.Int32)
IL_035e: nop
IL_035f: ldarg.0
IL_0360: call System.Single UnityEngine.Time::get_time()
IL_0365: ldc.r4 1
IL_036a: add
IL_036b: stfld System.Single CellDirector::spawnThrottleTime
IL_0370: ret

*/

// CellDirector
public void Update()
{
	object obj = null;
	if (!this.allowSpawns)
	{
		return;
	}
	if (Time.time >= this.spawnThrottleTime)
	{
		if (this.timeDir.HasReached(this.nextSpawn))
		{
			if (this.sector.Hibernate)
			{
				goto IL_2F4;
			}
			_hook_result hook_result = SiscosHooks.call(HOOK_ID.Pre_Region_Spawn_Cycle, this, ref obj, null);
			if (hook_result.abort)
			{
				return;
			}
			if (this.spawners.Count > 0)
			{
				if (this.NeedsMoreSlimes())
				{
					if (!this.HasTarrSlimes())
					{
						Dictionary<DirectedSlimeSpawner, float> dictionary = new Dictionary<DirectedSlimeSpawner, float>();
						float num = 0f;
						foreach (DirectedSlimeSpawner current in this.spawners)
						{
							if (current.CanSpawn(null))
							{
								dictionary[current] = current.directedSpawnWeight;
								num += current.directedSpawnWeight;
							}
						}
						if (dictionary.Count > 0 && num > 0f)
						{
							DirectedSlimeSpawner directedSlimeSpawner = this.rand.Pick<DirectedSlimeSpawner>(dictionary, null);
							float num2 = SRSingleton<SceneContext>.Instance.ModDirector.SlimeCountFactor();
							base.StartCoroutine(directedSlimeSpawner.Spawn(Mathf.RoundToInt((float)this.rand.GetInRange(this.minPerSpawn, this.maxPerSpawn + 1) * num2), this.rand));
						}
						goto IL_1A5;
					}
				}
			}
			if (this.HasTooManySlimes())
			{
				this.Despawn(this.slimes, Mathf.CeilToInt(0.1f * (float)this.targetSlimeCount));
			}
			IL_1A5:
			if (this.animalSpawners.Count > 0)
			{
				if (this.NeedsMoreAnimals())
				{
					List<DirectedAnimalSpawner> list = new List<DirectedAnimalSpawner>();
					foreach (DirectedAnimalSpawner current2 in this.animalSpawners)
					{
						if (current2.CanSpawn(null))
						{
							list.Add(current2);
						}
					}
					if (list.Count > 0)
					{
						DirectedAnimalSpawner directedAnimalSpawner = this.rand.Pick<DirectedAnimalSpawner>(list, null);
						base.StartCoroutine(directedAnimalSpawner.Spawn(this.rand.GetInRange(this.minAnimalPerSpawn, this.maxAnimalPerSpawn + 1), this.rand));
					}
					goto IL_29D;
				}
			}
			if (this.HasTooManyAnimals())
			{
				this.Despawn(this.animals, Mathf.CeilToInt(0.1f * (float)this.targetAnimalCount));
			}
			IL_29D:
			this.nextSpawn += this.avgSpawnTimeGameHours * 3600f * this.rand.GetInRange(0.5f, 1.5f);
		}
		_hook_result hook_result2 = SiscosHooks.call(HOOK_ID.Post_Region_Spawn_Cycle, this, ref obj, null);
		if (hook_result2.abort)
		{
			return;
		}
		IL_2F4:
		if (this.slimes.Count > this.cullSlimesLimit)
		{
			this.Despawn(this.slimes, this.slimes.Count - this.cullSlimesLimit);
		}
		if (this.animals.Count > this.cullAnimalsLimit)
		{
			this.Despawn(this.animals, this.animals.Count - this.cullAnimalsLimit);
		}
		this.spawnThrottleTime = Time.time + 1f;
	}
}

/*
IL_0000: ldnull
IL_0001: stloc V_2
IL_0005: nop
IL_0006: ldc.i4.4
IL_0007: newarr PurchaseUI/Purchasable
IL_000c: dup
IL_000d: ldc.i4.0
IL_000e: ldstr "m.upgrade.name.coop.walls"
IL_0013: ldarg.0
IL_0014: ldfld LandPlotUI/UpgradePurchaseItem CoopUI::walls
IL_0019: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_001e: ldarg.0
IL_001f: ldfld LandPlotUI/UpgradePurchaseItem CoopUI::walls
IL_0024: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_0029: ldstr "m.upgrade.desc.coop.walls"
IL_002e: ldarg.0
IL_002f: ldfld LandPlotUI/UpgradePurchaseItem CoopUI::walls
IL_0034: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_0039: ldc.i4 4001
IL_003e: newobj System.Void System.Nullable`1<PediaDirector/Id>::.ctor(!0)
IL_0043: ldarg.0
IL_0044: ldftn System.Void CoopUI::UpgradeWalls()
IL_004a: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_004f: ldarg.0
IL_0050: ldfld LandPlot LandPlotUI::activator
IL_0055: ldc.i4.1
IL_0056: callvirt System.Boolean LandPlot::HasUpgrade(LandPlot/Upgrade)
IL_005b: ldc.i4.0
IL_005c: ceq
IL_005e: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_0063: stelem.ref
IL_0064: dup
IL_0065: ldc.i4.1
IL_0066: ldstr "m.upgrade.name.coop.feeder"
IL_006b: ldarg.0
IL_006c: ldfld LandPlotUI/UpgradePurchaseItem CoopUI::feeder
IL_0071: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_0076: ldarg.0
IL_0077: ldfld LandPlotUI/UpgradePurchaseItem CoopUI::feeder
IL_007c: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_0081: ldstr "m.upgrade.desc.coop.feeder"
IL_0086: ldarg.0
IL_0087: ldfld LandPlotUI/UpgradePurchaseItem CoopUI::feeder
IL_008c: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_0091: ldc.i4 4001
IL_0096: newobj System.Void System.Nullable`1<PediaDirector/Id>::.ctor(!0)
IL_009b: ldarg.0
IL_009c: ldftn System.Void CoopUI::UpgradeFeeder()
IL_00a2: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_00a7: ldarg.0
IL_00a8: ldfld LandPlot LandPlotUI::activator
IL_00ad: ldc.i4.s 9
IL_00af: callvirt System.Boolean LandPlot::HasUpgrade(LandPlot/Upgrade)
IL_00b4: ldc.i4.0
IL_00b5: ceq
IL_00b7: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_00bc: stelem.ref
IL_00bd: dup
IL_00be: ldc.i4.2
IL_00bf: ldstr "m.upgrade.name.coop.vitamizer"
IL_00c4: ldarg.0
IL_00c5: ldfld LandPlotUI/UpgradePurchaseItem CoopUI::vitamizer
IL_00ca: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_00cf: ldarg.0
IL_00d0: ldfld LandPlotUI/UpgradePurchaseItem CoopUI::vitamizer
IL_00d5: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_00da: ldstr "m.upgrade.desc.coop.vitamizer"
IL_00df: ldarg.0
IL_00e0: ldfld LandPlotUI/UpgradePurchaseItem CoopUI::vitamizer
IL_00e5: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_00ea: ldc.i4 4001
IL_00ef: newobj System.Void System.Nullable`1<PediaDirector/Id>::.ctor(!0)
IL_00f4: ldarg.0
IL_00f5: ldftn System.Void CoopUI::UpgradeVitamizer()
IL_00fb: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_0100: ldarg.0
IL_0101: ldfld LandPlot LandPlotUI::activator
IL_0106: ldc.i4.s 10
IL_0108: callvirt System.Boolean LandPlot::HasUpgrade(LandPlot/Upgrade)
IL_010d: ldc.i4.0
IL_010e: ceq
IL_0110: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_0115: stelem.ref
IL_0116: dup
IL_0117: ldc.i4.3
IL_0118: ldstr "ui"
IL_011d: ldstr "b.demolish"
IL_0122: call System.String MessageUtil::Qualify(System.String,System.String)
IL_0127: ldarg.0
IL_0128: ldfld LandPlotUI/PlotPurchaseItem CoopUI::demolish
IL_012d: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_0132: ldarg.0
IL_0133: ldfld LandPlotUI/PlotPurchaseItem CoopUI::demolish
IL_0138: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_013d: ldstr "ui"
IL_0142: ldstr "m.desc.demolish"
IL_0147: call System.String MessageUtil::Qualify(System.String,System.String)
IL_014c: ldarg.0
IL_014d: ldfld LandPlotUI/PlotPurchaseItem CoopUI::demolish
IL_0152: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_0157: ldloca.s V_1
IL_0159: initobj System.Nullable`1<PediaDirector/Id>
IL_015f: ldloc.1
IL_0160: ldarg.0
IL_0161: ldftn System.Void CoopUI::Demolish()
IL_0167: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_016c: ldc.i4.1
IL_016d: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_0172: stelem.ref
IL_0173: stloc.0
IL_0174: call T SRSingleton`1<GameContext>::get_Instance()
IL_0179: callvirt UITemplates GameContext::get_UITemplates()
IL_017e: ldarg.0
IL_017f: ldfld UnityEngine.Sprite CoopUI::titleIcon
IL_0184: ldstr "t.coop"
IL_0189: ldloc.0
IL_018a: ldarg.0
IL_018b: dup
IL_018c: ldvirtftn System.Void BaseUI::Close()
IL_0192: newobj System.Void PurchaseUI/OnClose::.ctor(System.Object,System.IntPtr)
IL_0197: callvirt UnityEngine.GameObject UITemplates::CreatePurchaseUI(UnityEngine.Sprite,System.String,PurchaseUI/Purchasable[],PurchaseUI/OnClose)
IL_019c: box UnityEngine.GameObject
IL_01a1: stloc V_2
IL_01a5: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::Ext_Spawn_Plot_Upgrades_UI
IL_01aa: ldarg 
IL_01ae: ldloca V_2
IL_01b2: ldnull
IL_01b3: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_01b8: stloc V_3
IL_01bc: ldloc V_2
IL_01c0: unbox.any UnityEngine.GameObject
IL_01c5: ret

*/

// CoopUI
protected override GameObject CreatePurchaseUI()
{
	object obj = null;
	PurchaseUI.Purchasable[] purchasables = new PurchaseUI.Purchasable[]
	{
		new PurchaseUI.Purchasable("m.upgrade.name.coop.walls", this.walls.icon, this.walls.img, "m.upgrade.desc.coop.walls", this.walls.cost, new PediaDirector.Id?(PediaDirector.Id.COOP), new UnityAction(this.UpgradeWalls), !this.activator.HasUpgrade(LandPlot.Upgrade.WALLS)),
		new PurchaseUI.Purchasable("m.upgrade.name.coop.feeder", this.feeder.icon, this.feeder.img, "m.upgrade.desc.coop.feeder", this.feeder.cost, new PediaDirector.Id?(PediaDirector.Id.COOP), new UnityAction(this.UpgradeFeeder), !this.activator.HasUpgrade(LandPlot.Upgrade.FEEDER)),
		new PurchaseUI.Purchasable("m.upgrade.name.coop.vitamizer", this.vitamizer.icon, this.vitamizer.img, "m.upgrade.desc.coop.vitamizer", this.vitamizer.cost, new PediaDirector.Id?(PediaDirector.Id.COOP), new UnityAction(this.UpgradeVitamizer), !this.activator.HasUpgrade(LandPlot.Upgrade.VITAMIZER)),
		new PurchaseUI.Purchasable(MessageUtil.Qualify("ui", "b.demolish"), this.demolish.icon, this.demolish.img, MessageUtil.Qualify("ui", "m.desc.demolish"), this.demolish.cost, null, new UnityAction(this.Demolish), true)
	};
	obj = SRSingleton<GameContext>.Instance.UITemplates.CreatePurchaseUI(this.titleIcon, "t.coop", purchasables, new PurchaseUI.OnClose(this.Close));
	_hook_result hook_result = SiscosHooks.call(HOOK_ID.Ext_Spawn_Plot_Upgrades_UI, this, ref obj, null);
	return (GameObject)obj;
}

/*
IL_0000: ldnull
IL_0001: stloc V_2
IL_0005: nop
IL_0006: ldc.i4.7
IL_0007: newarr PurchaseUI/Purchasable
IL_000c: dup
IL_000d: ldc.i4.0
IL_000e: ldstr "m.upgrade.name.corral.walls"
IL_0013: ldarg.0
IL_0014: ldfld LandPlotUI/UpgradePurchaseItem CorralUI::walls
IL_0019: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_001e: ldarg.0
IL_001f: ldfld LandPlotUI/UpgradePurchaseItem CorralUI::walls
IL_0024: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_0029: ldstr "m.upgrade.desc.corral.walls"
IL_002e: ldarg.0
IL_002f: ldfld LandPlotUI/UpgradePurchaseItem CorralUI::walls
IL_0034: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_0039: ldc.i4 4000
IL_003e: newobj System.Void System.Nullable`1<PediaDirector/Id>::.ctor(!0)
IL_0043: ldarg.0
IL_0044: ldftn System.Void CorralUI::UpgradeWalls()
IL_004a: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_004f: ldarg.0
IL_0050: ldfld LandPlot LandPlotUI::activator
IL_0055: ldc.i4.1
IL_0056: callvirt System.Boolean LandPlot::HasUpgrade(LandPlot/Upgrade)
IL_005b: ldc.i4.0
IL_005c: ceq
IL_005e: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_0063: stelem.ref
IL_0064: dup
IL_0065: ldc.i4.1
IL_0066: ldstr "m.upgrade.name.corral.music_box"
IL_006b: ldarg.0
IL_006c: ldfld LandPlotUI/UpgradePurchaseItem CorralUI::musicBox
IL_0071: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_0076: ldarg.0
IL_0077: ldfld LandPlotUI/UpgradePurchaseItem CorralUI::musicBox
IL_007c: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_0081: ldstr "m.upgrade.desc.corral.music_box"
IL_0086: ldarg.0
IL_0087: ldfld LandPlotUI/UpgradePurchaseItem CorralUI::musicBox
IL_008c: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_0091: ldc.i4 4000
IL_0096: newobj System.Void System.Nullable`1<PediaDirector/Id>::.ctor(!0)
IL_009b: ldarg.0
IL_009c: ldftn System.Void CorralUI::UpgradeMusicBox()
IL_00a2: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_00a7: ldarg.0
IL_00a8: ldfld LandPlot LandPlotUI::activator
IL_00ad: ldc.i4.2
IL_00ae: callvirt System.Boolean LandPlot::HasUpgrade(LandPlot/Upgrade)
IL_00b3: ldc.i4.0
IL_00b4: ceq
IL_00b6: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_00bb: stelem.ref
IL_00bc: dup
IL_00bd: ldc.i4.2
IL_00be: ldstr "m.upgrade.name.corral.air_net"
IL_00c3: ldarg.0
IL_00c4: ldfld LandPlotUI/UpgradePurchaseItem CorralUI::airNet
IL_00c9: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_00ce: ldarg.0
IL_00cf: ldfld LandPlotUI/UpgradePurchaseItem CorralUI::airNet
IL_00d4: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_00d9: ldstr "m.upgrade.desc.corral.air_net"
IL_00de: ldarg.0
IL_00df: ldfld LandPlotUI/UpgradePurchaseItem CorralUI::airNet
IL_00e4: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_00e9: ldc.i4 4000
IL_00ee: newobj System.Void System.Nullable`1<PediaDirector/Id>::.ctor(!0)
IL_00f3: ldarg.0
IL_00f4: ldftn System.Void CorralUI::UpgradeAirNet()
IL_00fa: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_00ff: ldarg.0
IL_0100: ldfld LandPlot LandPlotUI::activator
IL_0105: ldc.i4.s 11
IL_0107: callvirt System.Boolean LandPlot::HasUpgrade(LandPlot/Upgrade)
IL_010c: ldc.i4.0
IL_010d: ceq
IL_010f: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_0114: stelem.ref
IL_0115: dup
IL_0116: ldc.i4.3
IL_0117: ldstr "m.upgrade.name.corral.solar_shield"
IL_011c: ldarg.0
IL_011d: ldfld LandPlotUI/UpgradePurchaseItem CorralUI::solarShield
IL_0122: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_0127: ldarg.0
IL_0128: ldfld LandPlotUI/UpgradePurchaseItem CorralUI::solarShield
IL_012d: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_0132: ldstr "m.upgrade.desc.corral.solar_shield"
IL_0137: ldarg.0
IL_0138: ldfld LandPlotUI/UpgradePurchaseItem CorralUI::solarShield
IL_013d: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_0142: ldc.i4 4000
IL_0147: newobj System.Void System.Nullable`1<PediaDirector/Id>::.ctor(!0)
IL_014c: ldarg.0
IL_014d: ldftn System.Void CorralUI::UpgradeSolarShield()
IL_0153: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_0158: ldarg.0
IL_0159: ldfld LandPlot LandPlotUI::activator
IL_015e: ldc.i4.s 13
IL_0160: callvirt System.Boolean LandPlot::HasUpgrade(LandPlot/Upgrade)
IL_0165: ldc.i4.0
IL_0166: ceq
IL_0168: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_016d: stelem.ref
IL_016e: dup
IL_016f: ldc.i4.4
IL_0170: ldstr "m.upgrade.name.corral.plort_collector"
IL_0175: ldarg.0
IL_0176: ldfld LandPlotUI/UpgradePurchaseItem CorralUI::plortCollector
IL_017b: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_0180: ldarg.0
IL_0181: ldfld LandPlotUI/UpgradePurchaseItem CorralUI::plortCollector
IL_0186: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_018b: ldstr "m.upgrade.desc.corral.plort_collector"
IL_0190: ldarg.0
IL_0191: ldfld LandPlotUI/UpgradePurchaseItem CorralUI::plortCollector
IL_0196: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_019b: ldc.i4 4000
IL_01a0: newobj System.Void System.Nullable`1<PediaDirector/Id>::.ctor(!0)
IL_01a5: ldarg.0
IL_01a6: ldftn System.Void CorralUI::UpgradePlortCollector()
IL_01ac: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_01b1: ldarg.0
IL_01b2: ldfld LandPlot LandPlotUI::activator
IL_01b7: ldc.i4.s 12
IL_01b9: callvirt System.Boolean LandPlot::HasUpgrade(LandPlot/Upgrade)
IL_01be: ldc.i4.0
IL_01bf: ceq
IL_01c1: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_01c6: stelem.ref
IL_01c7: dup
IL_01c8: ldc.i4.5
IL_01c9: ldstr "m.upgrade.name.corral.feeder"
IL_01ce: ldarg.0
IL_01cf: ldfld LandPlotUI/UpgradePurchaseItem CorralUI::feeder
IL_01d4: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_01d9: ldarg.0
IL_01da: ldfld LandPlotUI/UpgradePurchaseItem CorralUI::feeder
IL_01df: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_01e4: ldstr "m.upgrade.desc.corral.feeder"
IL_01e9: ldarg.0
IL_01ea: ldfld LandPlotUI/UpgradePurchaseItem CorralUI::feeder
IL_01ef: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_01f4: ldc.i4 4000
IL_01f9: newobj System.Void System.Nullable`1<PediaDirector/Id>::.ctor(!0)
IL_01fe: ldarg.0
IL_01ff: ldftn System.Void CorralUI::UpgradeFeeder()
IL_0205: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_020a: ldarg.0
IL_020b: ldfld LandPlot LandPlotUI::activator
IL_0210: ldc.i4.s 9
IL_0212: callvirt System.Boolean LandPlot::HasUpgrade(LandPlot/Upgrade)
IL_0217: ldc.i4.0
IL_0218: ceq
IL_021a: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_021f: stelem.ref
IL_0220: dup
IL_0221: ldc.i4.6
IL_0222: ldstr "ui"
IL_0227: ldstr "b.demolish"
IL_022c: call System.String MessageUtil::Qualify(System.String,System.String)
IL_0231: ldarg.0
IL_0232: ldfld LandPlotUI/PlotPurchaseItem CorralUI::demolish
IL_0237: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_023c: ldarg.0
IL_023d: ldfld LandPlotUI/PlotPurchaseItem CorralUI::demolish
IL_0242: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_0247: ldstr "ui"
IL_024c: ldstr "m.desc.demolish"
IL_0251: call System.String MessageUtil::Qualify(System.String,System.String)
IL_0256: ldarg.0
IL_0257: ldfld LandPlotUI/PlotPurchaseItem CorralUI::demolish
IL_025c: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_0261: ldloca.s V_1
IL_0263: initobj System.Nullable`1<PediaDirector/Id>
IL_0269: ldloc.1
IL_026a: ldarg.0
IL_026b: ldftn System.Void CorralUI::Demolish()
IL_0271: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_0276: ldc.i4.1
IL_0277: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_027c: stelem.ref
IL_027d: stloc.0
IL_027e: call T SRSingleton`1<GameContext>::get_Instance()
IL_0283: callvirt UITemplates GameContext::get_UITemplates()
IL_0288: ldarg.0
IL_0289: ldfld UnityEngine.Sprite CorralUI::titleIcon
IL_028e: ldstr "t.corral"
IL_0293: ldloc.0
IL_0294: ldarg.0
IL_0295: dup
IL_0296: ldvirtftn System.Void BaseUI::Close()
IL_029c: newobj System.Void PurchaseUI/OnClose::.ctor(System.Object,System.IntPtr)
IL_02a1: callvirt UnityEngine.GameObject UITemplates::CreatePurchaseUI(UnityEngine.Sprite,System.String,PurchaseUI/Purchasable[],PurchaseUI/OnClose)
IL_02a6: box UnityEngine.GameObject
IL_02ab: stloc V_2
IL_02af: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::Ext_Spawn_Plot_Upgrades_UI
IL_02b4: ldarg 
IL_02b8: ldloca V_2
IL_02bc: ldnull
IL_02bd: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_02c2: stloc V_3
IL_02c6: ldloc V_2
IL_02ca: unbox.any UnityEngine.GameObject
IL_02cf: ret

*/

// CorralUI
protected override GameObject CreatePurchaseUI()
{
	object obj = null;
	PurchaseUI.Purchasable[] purchasables = new PurchaseUI.Purchasable[]
	{
		new PurchaseUI.Purchasable("m.upgrade.name.corral.walls", this.walls.icon, this.walls.img, "m.upgrade.desc.corral.walls", this.walls.cost, new PediaDirector.Id?(PediaDirector.Id.CORRAL), new UnityAction(this.UpgradeWalls), !this.activator.HasUpgrade(LandPlot.Upgrade.WALLS)),
		new PurchaseUI.Purchasable("m.upgrade.name.corral.music_box", this.musicBox.icon, this.musicBox.img, "m.upgrade.desc.corral.music_box", this.musicBox.cost, new PediaDirector.Id?(PediaDirector.Id.CORRAL), new UnityAction(this.UpgradeMusicBox), !this.activator.HasUpgrade(LandPlot.Upgrade.MUSIC_BOX)),
		new PurchaseUI.Purchasable("m.upgrade.name.corral.air_net", this.airNet.icon, this.airNet.img, "m.upgrade.desc.corral.air_net", this.airNet.cost, new PediaDirector.Id?(PediaDirector.Id.CORRAL), new UnityAction(this.UpgradeAirNet), !this.activator.HasUpgrade(LandPlot.Upgrade.AIR_NET)),
		new PurchaseUI.Purchasable("m.upgrade.name.corral.solar_shield", this.solarShield.icon, this.solarShield.img, "m.upgrade.desc.corral.solar_shield", this.solarShield.cost, new PediaDirector.Id?(PediaDirector.Id.CORRAL), new UnityAction(this.UpgradeSolarShield), !this.activator.HasUpgrade(LandPlot.Upgrade.SOLAR_SHIELD)),
		new PurchaseUI.Purchasable("m.upgrade.name.corral.plort_collector", this.plortCollector.icon, this.plortCollector.img, "m.upgrade.desc.corral.plort_collector", this.plortCollector.cost, new PediaDirector.Id?(PediaDirector.Id.CORRAL), new UnityAction(this.UpgradePlortCollector), !this.activator.HasUpgrade(LandPlot.Upgrade.PLORT_COLLECTOR)),
		new PurchaseUI.Purchasable("m.upgrade.name.corral.feeder", this.feeder.icon, this.feeder.img, "m.upgrade.desc.corral.feeder", this.feeder.cost, new PediaDirector.Id?(PediaDirector.Id.CORRAL), new UnityAction(this.UpgradeFeeder), !this.activator.HasUpgrade(LandPlot.Upgrade.FEEDER)),
		new PurchaseUI.Purchasable(MessageUtil.Qualify("ui", "b.demolish"), this.demolish.icon, this.demolish.img, MessageUtil.Qualify("ui", "m.desc.demolish"), this.demolish.cost, null, new UnityAction(this.Demolish), true)
	};
	obj = SRSingleton<GameContext>.Instance.UITemplates.CreatePurchaseUI(this.titleIcon, "t.corral", purchasables, new PurchaseUI.OnClose(this.Close));
	_hook_result hook_result = SiscosHooks.call(HOOK_ID.Ext_Spawn_Plot_Upgrades_UI, this, ref obj, null);
	return (GameObject)obj;
}

/*
IL_0000: ldnull
IL_0001: stloc V_1
IL_0005: nop
IL_0006: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::Pre_Entity_Spawn
IL_000b: ldarg 
IL_000f: ldloca V_1
IL_0013: ldc.i4 2
IL_0018: newarr System.Object
IL_001d: dup
IL_001e: ldc.i4 0
IL_0023: ldarg count
IL_0027: box System.Int32
IL_002c: stelem.ref
IL_002d: dup
IL_002e: ldc.i4 1
IL_0033: ldarg rand
IL_0037: box Randoms
IL_003c: stelem.ref
IL_003d: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_0042: stloc V_2
IL_0046: ldloc V_2
IL_004a: ldfld System.Boolean SR_PluginLoader._hook_result::abort
IL_004f: brfalse IL_005e
IL_0054: ldloc V_1
IL_0058: unbox.any System.Collections.IEnumerator
IL_005d: ret
IL_005e: nop
IL_005f: ldloc V_2
IL_0063: ldfld System.Object[] SR_PluginLoader._hook_result::args
IL_0068: brfalse IL_0099
IL_006d: ldloc V_2
IL_0071: ldfld System.Object[] SR_PluginLoader._hook_result::args
IL_0076: ldc.i4 0
IL_007b: ldelem.ref
IL_007c: unbox.any System.Int32
IL_0081: starg.s count
IL_0083: ldloc V_2
IL_0087: ldfld System.Object[] SR_PluginLoader._hook_result::args
IL_008c: ldc.i4 1
IL_0091: ldelem.ref
IL_0092: unbox.any Randoms
IL_0097: starg.s rand
IL_0099: nop
IL_009a: newobj System.Void DirectedActorSpawner/<Spawn>c__Iterator1F::.ctor()
IL_009f: stloc.0
IL_00a0: ldloc.0
IL_00a1: ldarg.2
IL_00a2: stfld Randoms DirectedActorSpawner/<Spawn>c__Iterator1F::rand
IL_00a7: ldloc.0
IL_00a8: ldarg.1
IL_00a9: stfld System.Int32 DirectedActorSpawner/<Spawn>c__Iterator1F::count
IL_00ae: ldloc.0
IL_00af: ldarg.2
IL_00b0: stfld Randoms DirectedActorSpawner/<Spawn>c__Iterator1F::<$>rand
IL_00b5: ldloc.0
IL_00b6: ldarg.1
IL_00b7: stfld System.Int32 DirectedActorSpawner/<Spawn>c__Iterator1F::<$>count
IL_00bc: ldloc.0
IL_00bd: ldarg.0
IL_00be: stfld DirectedActorSpawner DirectedActorSpawner/<Spawn>c__Iterator1F::<>f__this
IL_00c3: ldloc.0
IL_00c4: ret

*/

// DirectedActorSpawner
[DebuggerHidden]
public virtual IEnumerator Spawn(int count, Randoms rand)
{
	object obj = null;
	_hook_result hook_result = SiscosHooks.call(HOOK_ID.Pre_Entity_Spawn, this, ref obj, new object[]
	{
		count,
		rand
	});
	if (hook_result.abort)
	{
		return (IEnumerator)obj;
	}
	if (hook_result.args != null)
	{
		count = (int)hook_result.args[0];
		rand = (Randoms)hook_result.args[1];
	}
	DirectedActorSpawner.<Spawn>c__Iterator1F <Spawn>c__Iterator1F = new DirectedActorSpawner.<Spawn>c__Iterator1F();
	<Spawn>c__Iterator1F.rand = rand;
	<Spawn>c__Iterator1F.count = count;
	<Spawn>c__Iterator1F.<$>rand = rand;
	<Spawn>c__Iterator1F.<$>count = count;
	<Spawn>c__Iterator1F.<>f__this = this;
	return <Spawn>c__Iterator1F;
}

/*
IL_0000: ldnull
IL_0001: stloc V_0
IL_0005: nop
IL_0006: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::EntitySpawner_Init
IL_000b: ldarg 
IL_000f: ldloca V_0
IL_0013: ldnull
IL_0014: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_0019: stloc V_1
IL_001d: ldloc V_1
IL_0021: ldfld System.Boolean SR_PluginLoader._hook_result::abort
IL_0026: brfalse IL_002c
IL_002b: ret
IL_002c: nop
IL_002d: ldarg.0
IL_002e: call T SRSingleton`1<SceneContext>::get_Instance()
IL_0033: callvirt TimeDirector SceneContext::get_TimeDirector()
IL_0038: stfld TimeDirector DirectedActorSpawner::timeDir
IL_003d: ldarg.0
IL_003e: ldarg.0
IL_003f: call !!0 UnityEngine.Component::GetComponentInParent<CellDirector>()
IL_0044: stfld CellDirector DirectedActorSpawner::cellDir
IL_0049: ldarg.0
IL_004a: ldarg.0
IL_004b: ldfld CellDirector DirectedActorSpawner::cellDir
IL_0050: callvirt System.Void DirectedActorSpawner::Register(CellDirector)
IL_0055: ret

*/

// DirectedActorSpawner
public virtual void Start()
{
	object obj = null;
	_hook_result hook_result = SiscosHooks.call(HOOK_ID.EntitySpawner_Init, this, ref obj, null);
	if (hook_result.abort)
	{
		return;
	}
	this.timeDir = SRSingleton<SceneContext>.Instance.TimeDirector;
	this.cellDir = base.GetComponentInParent<CellDirector>();
	this.Register(this.cellDir);
}

/*
IL_0000: ldnull
IL_0001: stloc V_3
IL_0005: nop
IL_0006: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::Pre_Economy_Init
IL_000b: ldarg 
IL_000f: ldloca V_3
IL_0013: ldnull
IL_0014: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_0019: stloc V_4
IL_001d: ldloc V_4
IL_0021: ldfld System.Boolean SR_PluginLoader._hook_result::abort
IL_0026: brfalse IL_002c
IL_002b: ret
IL_002c: nop
IL_002d: ldarg.0
IL_002e: call T SRSingleton`1<SceneContext>::get_Instance()
IL_0033: callvirt TimeDirector SceneContext::get_TimeDirector()
IL_0038: stfld TimeDirector EconomyDirector::timeDir
IL_003d: ldarg.0
IL_003e: call T SRSingleton`1<SceneContext>::get_Instance()
IL_0043: callvirt PlayerState SceneContext::get_PlayerState()
IL_0048: stfld PlayerState EconomyDirector::playerState
IL_004d: ldarg.0
IL_004e: ldc.r4 0
IL_0053: stfld System.Single EconomyDirector::nextUpdateTime
IL_0058: ldarg.0
IL_0059: ldfld System.Single EconomyDirector::saturationRecovery
IL_005e: ldc.r4 0
IL_0063: blt IL_0078
IL_0068: ldarg.0
IL_0069: ldfld System.Single EconomyDirector::saturationRecovery
IL_006e: ldc.r4 1
IL_0073: ble.un IL_0084
IL_0078: nop
IL_0079: ldstr "Saturation Recovery must be [0-1]"
IL_007e: newobj System.Void System.ArgumentException::.ctor(System.String)
IL_0083: throw
IL_0084: nop
IL_0085: ldarg.0
IL_0086: newobj System.Void Randoms::.ctor()
IL_008b: ldc.r4 1000000
IL_0090: callvirt System.Single Randoms::GetFloat(System.Single)
IL_0095: stfld System.Single EconomyDirector::noiseSeed
IL_009a: ldarg.0
IL_009b: ldfld EconomyDirector/ValueMap[] EconomyDirector::baseValueMap
IL_00a0: stloc.1
IL_00a1: ldc.i4.0
IL_00a2: stloc.2
IL_00a3: br IL_0106
IL_00a8: nop
IL_00a9: ldloc.1
IL_00aa: ldloc.2
IL_00ab: ldelem.ref
IL_00ac: stloc.0
IL_00ad: ldarg.0
IL_00ae: ldfld System.Collections.Generic.Dictionary`2<Identifiable/Id,EconomyDirector/CurrValueEntry> EconomyDirector::currValueMap
IL_00b3: ldloc.0
IL_00b4: ldfld Identifiable EconomyDirector/ValueMap::accept
IL_00b9: ldfld Identifiable/Id Identifiable::id
IL_00be: ldloc.0
IL_00bf: ldfld System.Single EconomyDirector/ValueMap::value
IL_00c4: ldloc.0
IL_00c5: ldfld System.Single EconomyDirector/ValueMap::value
IL_00ca: ldloc.0
IL_00cb: ldfld System.Single EconomyDirector/ValueMap::value
IL_00d0: ldloc.0
IL_00d1: ldfld System.Single EconomyDirector/ValueMap::fullSaturation
IL_00d6: newobj System.Void EconomyDirector/CurrValueEntry::.ctor(System.Single,System.Single,System.Single,System.Single)
IL_00db: callvirt System.Void System.Collections.Generic.Dictionary`2<Identifiable/Id,EconomyDirector/CurrValueEntry>::set_Item(!0,!1)
IL_00e0: ldarg.0
IL_00e1: ldfld System.Collections.Generic.Dictionary`2<Identifiable/Id,System.Single> EconomyDirector::marketSaturation
IL_00e6: ldloc.0
IL_00e7: ldfld Identifiable EconomyDirector/ValueMap::accept
IL_00ec: ldfld Identifiable/Id Identifiable::id
IL_00f1: ldloc.0
IL_00f2: ldfld System.Single EconomyDirector/ValueMap::fullSaturation
IL_00f7: ldc.r4 0.5
IL_00fc: mul
IL_00fd: callvirt System.Void System.Collections.Generic.Dictionary`2<Identifiable/Id,System.Single>::set_Item(!0,!1)
IL_0102: ldloc.2
IL_0103: ldc.i4.1
IL_0104: add
IL_0105: stloc.2
IL_0106: ldloc.2
IL_0107: ldloc.1
IL_0108: ldlen
IL_0109: conv.i4
IL_010a: blt IL_00a8
IL_010f: nop
IL_0110: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::Post_Economy_Init
IL_0115: ldarg 
IL_0119: ldloca V_3
IL_011d: ldnull
IL_011e: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_0123: pop
IL_0124: ret

*/

// EconomyDirector
public void InitForLevel()
{
	object obj = null;
	_hook_result hook_result = SiscosHooks.call(HOOK_ID.Pre_Economy_Init, this, ref obj, null);
	if (hook_result.abort)
	{
		return;
	}
	this.timeDir = SRSingleton<SceneContext>.Instance.TimeDirector;
	this.playerState = SRSingleton<SceneContext>.Instance.PlayerState;
	this.nextUpdateTime = 0f;
	if (this.saturationRecovery < 0f || this.saturationRecovery > 1f)
	{
		throw new ArgumentException("Saturation Recovery must be [0-1]");
	}
	this.noiseSeed = new Randoms().GetFloat(1000000f);
	EconomyDirector.ValueMap[] array = this.baseValueMap;
	for (int i = 0; i < array.Length; i++)
	{
		EconomyDirector.ValueMap valueMap = array[i];
		this.currValueMap[valueMap.accept.id] = new EconomyDirector.CurrValueEntry(valueMap.value, valueMap.value, valueMap.value, valueMap.fullSaturation);
		this.marketSaturation[valueMap.accept.id] = valueMap.fullSaturation * 0.5f;
	}
	SiscosHooks.call(HOOK_ID.Post_Economy_Init, this, ref obj, null);
}

/*
IL_0000: ldnull
IL_0001: stloc V_7
IL_0005: nop
IL_0006: ldarg.0
IL_0007: ldfld TimeDirector EconomyDirector::timeDir
IL_000c: ldarg.0
IL_000d: ldfld System.Single EconomyDirector::nextUpdateTime
IL_0012: callvirt System.Boolean TimeDirector::HasReached(System.Single)
IL_0017: brfalse IL_012f
IL_001c: ldarg.0
IL_001d: ldfld TimeDirector EconomyDirector::timeDir
IL_0022: callvirt System.Int32 TimeDirector::CurrDay()
IL_0027: stloc.0
IL_0028: ldarg.0
IL_0029: ldfld System.Collections.Generic.Dictionary`2<Identifiable/Id,EconomyDirector/CurrValueEntry> EconomyDirector::currValueMap
IL_002e: callvirt System.Collections.Generic.Dictionary`2/Enumerator<!0,!1> System.Collections.Generic.Dictionary`2<Identifiable/Id,EconomyDirector/CurrValueEntry>::GetEnumerator()
IL_0033: stloc.2
IL_0034: br IL_00d2
IL_0039: nop
IL_003a: ldloca.s V_2
IL_003c: call System.Collections.Generic.KeyValuePair`2<!0,!1> System.Collections.Generic.Dictionary`2/Enumerator<Identifiable/Id,EconomyDirector/CurrValueEntry>::get_Current()
IL_0041: stloc.1
IL_0042: ldarg.0
IL_0043: ldfld System.Single EconomyDirector::nextUpdateTime
IL_0048: ldc.r4 0
IL_004d: ble.un IL_0084
IL_0052: ldarg.0
IL_0053: ldfld System.Collections.Generic.Dictionary`2<Identifiable/Id,System.Single> EconomyDirector::marketSaturation
IL_0058: dup
IL_0059: stloc.s V_4
IL_005b: ldloca.s V_1
IL_005d: call !0 System.Collections.Generic.KeyValuePair`2<Identifiable/Id,EconomyDirector/CurrValueEntry>::get_Key()
IL_0062: dup
IL_0063: stloc.s V_5
IL_0065: ldloc.s V_4
IL_0067: ldloc.s V_5
IL_0069: callvirt !1 System.Collections.Generic.Dictionary`2<Identifiable/Id,System.Single>::get_Item(!0)
IL_006e: stloc.s V_6
IL_0070: ldloc.s V_6
IL_0072: ldc.r4 1
IL_0077: ldarg.0
IL_0078: ldfld System.Single EconomyDirector::saturationRecovery
IL_007d: sub
IL_007e: mul
IL_007f: callvirt System.Void System.Collections.Generic.Dictionary`2<Identifiable/Id,System.Single>::set_Item(!0,!1)
IL_0084: nop
IL_0085: ldarg.0
IL_0086: ldloca.s V_1
IL_0088: call !0 System.Collections.Generic.KeyValuePair`2<Identifiable/Id,EconomyDirector/CurrValueEntry>::get_Key()
IL_008d: ldloca.s V_1
IL_008f: call !1 System.Collections.Generic.KeyValuePair`2<Identifiable/Id,EconomyDirector/CurrValueEntry>::get_Value()
IL_0094: ldfld System.Single EconomyDirector/CurrValueEntry::baseValue
IL_0099: ldloca.s V_1
IL_009b: call !1 System.Collections.Generic.KeyValuePair`2<Identifiable/Id,EconomyDirector/CurrValueEntry>::get_Value()
IL_00a0: ldfld System.Single EconomyDirector/CurrValueEntry::fullSaturation
IL_00a5: ldloc.0
IL_00a6: conv.r4
IL_00a7: call System.Single EconomyDirector::GetTargetValue(Identifiable/Id,System.Single,System.Single,System.Single)
IL_00ac: stloc.3
IL_00ad: ldloca.s V_1
IL_00af: call !1 System.Collections.Generic.KeyValuePair`2<Identifiable/Id,EconomyDirector/CurrValueEntry>::get_Value()
IL_00b4: ldloca.s V_1
IL_00b6: call !1 System.Collections.Generic.KeyValuePair`2<Identifiable/Id,EconomyDirector/CurrValueEntry>::get_Value()
IL_00bb: ldfld System.Single EconomyDirector/CurrValueEntry::currValue
IL_00c0: stfld System.Single EconomyDirector/CurrValueEntry::prevValue
IL_00c5: ldloca.s V_1
IL_00c7: call !1 System.Collections.Generic.KeyValuePair`2<Identifiable/Id,EconomyDirector/CurrValueEntry>::get_Value()
IL_00cc: ldloc.3
IL_00cd: stfld System.Single EconomyDirector/CurrValueEntry::currValue
IL_00d2: ldloca.s V_2
IL_00d4: call System.Boolean System.Collections.Generic.Dictionary`2/Enumerator<Identifiable/Id,EconomyDirector/CurrValueEntry>::MoveNext()
IL_00d9: brtrue IL_0039
IL_00de: leave IL_00ef
IL_00e3: ldloc.2
IL_00e4: box System.Collections.Generic.Dictionary`2/Enumerator<Identifiable/Id,EconomyDirector/CurrValueEntry>
IL_00e9: callvirt System.Void System.IDisposable::Dispose()
IL_00ee: endfinally
IL_00ef: ldarg.0
IL_00f0: ldarg.0
IL_00f1: ldfld TimeDirector EconomyDirector::timeDir
IL_00f6: ldc.r4 0
IL_00fb: callvirt System.Single TimeDirector::GetNextHour(System.Single)
IL_0100: stfld System.Single EconomyDirector::nextUpdateTime
IL_0105: ldarg.0
IL_0106: ldfld EconomyDirector/DidUpdate EconomyDirector::didUpdateDelegate
IL_010b: brfalse IL_012f
IL_0110: ldarg.0
IL_0111: ldfld EconomyDirector/DidUpdate EconomyDirector::didUpdateDelegate
IL_0116: callvirt System.Void EconomyDirector/DidUpdate::Invoke()
IL_011b: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::Economy_Updated
IL_0120: ldarg 
IL_0124: ldloca V_7
IL_0128: ldnull
IL_0129: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_012e: pop
IL_012f: nop
IL_0130: ret

*/

// EconomyDirector
public void Update()
{
	object obj = null;
	if (this.timeDir.HasReached(this.nextUpdateTime))
	{
		int num = this.timeDir.CurrDay();
		foreach (KeyValuePair<Identifiable.Id, EconomyDirector.CurrValueEntry> current in this.currValueMap)
		{
			if (this.nextUpdateTime > 0f)
			{
				Dictionary<Identifiable.Id, float> dictionary;
				Dictionary<Identifiable.Id, float> expr_58 = dictionary = this.marketSaturation;
				Identifiable.Id key;
				Identifiable.Id expr_62 = key = current.Key;
				float num2 = dictionary[key];
				expr_58[expr_62] = num2 * (1f - this.saturationRecovery);
			}
			float targetValue = this.GetTargetValue(current.Key, current.Value.baseValue, current.Value.fullSaturation, (float)num);
			current.Value.prevValue = current.Value.currValue;
			current.Value.currValue = targetValue;
		}
		this.nextUpdateTime = this.timeDir.GetNextHour(0f);
		if (this.didUpdateDelegate != null)
		{
			this.didUpdateDelegate();
			SiscosHooks.call(HOOK_ID.Economy_Updated, this, ref obj, null);
		}
	}
}

/*
IL_0000: ldnull
IL_0001: stloc V_1
IL_0005: nop
IL_0006: ldc.i4.6
IL_0007: newarr PurchaseUI/Purchasable
IL_000c: dup
IL_000d: ldc.i4.0
IL_000e: ldstr "t.corral"
IL_0013: ldarg.0
IL_0014: ldfld LandPlotUI/PlotPurchaseItem EmptyPlotUI::corral
IL_0019: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_001e: ldarg.0
IL_001f: ldfld LandPlotUI/PlotPurchaseItem EmptyPlotUI::corral
IL_0024: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_0029: ldstr "m.intro.corral"
IL_002e: ldarg.0
IL_002f: ldfld LandPlotUI/PlotPurchaseItem EmptyPlotUI::corral
IL_0034: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_0039: ldc.i4 4000
IL_003e: newobj System.Void System.Nullable`1<PediaDirector/Id>::.ctor(!0)
IL_0043: ldarg.0
IL_0044: ldftn System.Void EmptyPlotUI::BuyCorral()
IL_004a: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_004f: ldc.i4.1
IL_0050: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_0055: stelem.ref
IL_0056: dup
IL_0057: ldc.i4.1
IL_0058: ldstr "t.garden"
IL_005d: ldarg.0
IL_005e: ldfld LandPlotUI/PlotPurchaseItem EmptyPlotUI::garden
IL_0063: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_0068: ldarg.0
IL_0069: ldfld LandPlotUI/PlotPurchaseItem EmptyPlotUI::garden
IL_006e: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_0073: ldstr "m.intro.garden"
IL_0078: ldarg.0
IL_0079: ldfld LandPlotUI/PlotPurchaseItem EmptyPlotUI::garden
IL_007e: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_0083: ldc.i4 4002
IL_0088: newobj System.Void System.Nullable`1<PediaDirector/Id>::.ctor(!0)
IL_008d: ldarg.0
IL_008e: ldftn System.Void EmptyPlotUI::BuyGarden()
IL_0094: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_0099: ldc.i4.1
IL_009a: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_009f: stelem.ref
IL_00a0: dup
IL_00a1: ldc.i4.2
IL_00a2: ldstr "t.coop"
IL_00a7: ldarg.0
IL_00a8: ldfld LandPlotUI/PlotPurchaseItem EmptyPlotUI::coop
IL_00ad: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_00b2: ldarg.0
IL_00b3: ldfld LandPlotUI/PlotPurchaseItem EmptyPlotUI::coop
IL_00b8: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_00bd: ldstr "m.intro.coop"
IL_00c2: ldarg.0
IL_00c3: ldfld LandPlotUI/PlotPurchaseItem EmptyPlotUI::coop
IL_00c8: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_00cd: ldc.i4 4001
IL_00d2: newobj System.Void System.Nullable`1<PediaDirector/Id>::.ctor(!0)
IL_00d7: ldarg.0
IL_00d8: ldftn System.Void EmptyPlotUI::BuyCoop()
IL_00de: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_00e3: ldc.i4.1
IL_00e4: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_00e9: stelem.ref
IL_00ea: dup
IL_00eb: ldc.i4.3
IL_00ec: ldstr "t.silo"
IL_00f1: ldarg.0
IL_00f2: ldfld LandPlotUI/PlotPurchaseItem EmptyPlotUI::silo
IL_00f7: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_00fc: ldarg.0
IL_00fd: ldfld LandPlotUI/PlotPurchaseItem EmptyPlotUI::silo
IL_0102: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_0107: ldstr "m.intro.silo"
IL_010c: ldarg.0
IL_010d: ldfld LandPlotUI/PlotPurchaseItem EmptyPlotUI::silo
IL_0112: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_0117: ldc.i4 4003
IL_011c: newobj System.Void System.Nullable`1<PediaDirector/Id>::.ctor(!0)
IL_0121: ldarg.0
IL_0122: ldftn System.Void EmptyPlotUI::BuySilo()
IL_0128: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_012d: ldc.i4.1
IL_012e: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_0133: stelem.ref
IL_0134: dup
IL_0135: ldc.i4.4
IL_0136: ldstr "t.incinerator"
IL_013b: ldarg.0
IL_013c: ldfld LandPlotUI/PlotPurchaseItem EmptyPlotUI::incinerator
IL_0141: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_0146: ldarg.0
IL_0147: ldfld LandPlotUI/PlotPurchaseItem EmptyPlotUI::incinerator
IL_014c: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_0151: ldstr "m.intro.incinerator"
IL_0156: ldarg.0
IL_0157: ldfld LandPlotUI/PlotPurchaseItem EmptyPlotUI::incinerator
IL_015c: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_0161: ldc.i4 4004
IL_0166: newobj System.Void System.Nullable`1<PediaDirector/Id>::.ctor(!0)
IL_016b: ldarg.0
IL_016c: ldftn System.Void EmptyPlotUI::BuyIncinerator()
IL_0172: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_0177: ldc.i4.1
IL_0178: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_017d: stelem.ref
IL_017e: dup
IL_017f: ldc.i4.5
IL_0180: ldstr "t.pond"
IL_0185: ldarg.0
IL_0186: ldfld LandPlotUI/PlotPurchaseItem EmptyPlotUI::pond
IL_018b: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_0190: ldarg.0
IL_0191: ldfld LandPlotUI/PlotPurchaseItem EmptyPlotUI::pond
IL_0196: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_019b: ldstr "m.intro.pond"
IL_01a0: ldarg.0
IL_01a1: ldfld LandPlotUI/PlotPurchaseItem EmptyPlotUI::pond
IL_01a6: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_01ab: ldc.i4 4005
IL_01b0: newobj System.Void System.Nullable`1<PediaDirector/Id>::.ctor(!0)
IL_01b5: ldarg.0
IL_01b6: ldftn System.Void EmptyPlotUI::BuyPond()
IL_01bc: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_01c1: ldc.i4.1
IL_01c2: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_01c7: stelem.ref
IL_01c8: stloc.0
IL_01c9: call T SRSingleton`1<GameContext>::get_Instance()
IL_01ce: callvirt UITemplates GameContext::get_UITemplates()
IL_01d3: ldarg.0
IL_01d4: ldfld UnityEngine.Sprite EmptyPlotUI::titleIcon
IL_01d9: ldstr "ui"
IL_01de: ldstr "t.empty_plot"
IL_01e3: call System.String MessageUtil::Qualify(System.String,System.String)
IL_01e8: ldloc.0
IL_01e9: ldarg.0
IL_01ea: dup
IL_01eb: ldvirtftn System.Void BaseUI::Close()
IL_01f1: newobj System.Void PurchaseUI/OnClose::.ctor(System.Object,System.IntPtr)
IL_01f6: callvirt UnityEngine.GameObject UITemplates::CreatePurchaseUI(UnityEngine.Sprite,System.String,PurchaseUI/Purchasable[],PurchaseUI/OnClose)
IL_01fb: box UnityEngine.GameObject
IL_0200: stloc V_1
IL_0204: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::Ext_Spawn_Plot_Upgrades_UI
IL_0209: ldarg 
IL_020d: ldloca V_1
IL_0211: ldnull
IL_0212: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_0217: stloc V_2
IL_021b: ldloc V_1
IL_021f: unbox.any UnityEngine.GameObject
IL_0224: ret

*/

// EmptyPlotUI
protected override GameObject CreatePurchaseUI()
{
	object obj = null;
	PurchaseUI.Purchasable[] purchasables = new PurchaseUI.Purchasable[]
	{
		new PurchaseUI.Purchasable("t.corral", this.corral.icon, this.corral.img, "m.intro.corral", this.corral.cost, new PediaDirector.Id?(PediaDirector.Id.CORRAL), new UnityAction(this.BuyCorral), true),
		new PurchaseUI.Purchasable("t.garden", this.garden.icon, this.garden.img, "m.intro.garden", this.garden.cost, new PediaDirector.Id?(PediaDirector.Id.GARDEN), new UnityAction(this.BuyGarden), true),
		new PurchaseUI.Purchasable("t.coop", this.coop.icon, this.coop.img, "m.intro.coop", this.coop.cost, new PediaDirector.Id?(PediaDirector.Id.COOP), new UnityAction(this.BuyCoop), true),
		new PurchaseUI.Purchasable("t.silo", this.silo.icon, this.silo.img, "m.intro.silo", this.silo.cost, new PediaDirector.Id?(PediaDirector.Id.SILO), new UnityAction(this.BuySilo), true),
		new PurchaseUI.Purchasable("t.incinerator", this.incinerator.icon, this.incinerator.img, "m.intro.incinerator", this.incinerator.cost, new PediaDirector.Id?(PediaDirector.Id.INCINERATOR), new UnityAction(this.BuyIncinerator), true),
		new PurchaseUI.Purchasable("t.pond", this.pond.icon, this.pond.img, "m.intro.pond", this.pond.cost, new PediaDirector.Id?(PediaDirector.Id.POND), new UnityAction(this.BuyPond), true)
	};
	obj = SRSingleton<GameContext>.Instance.UITemplates.CreatePurchaseUI(this.titleIcon, MessageUtil.Qualify("ui", "t.empty_plot"), purchasables, new PurchaseUI.OnClose(this.Close));
	_hook_result hook_result = SiscosHooks.call(HOOK_ID.Ext_Spawn_Plot_Upgrades_UI, this, ref obj, null);
	return (GameObject)obj;
}

/*
IL_0000: ldnull
IL_0001: stloc V_6
IL_0005: nop
IL_0006: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::Get_Available_Saves
IL_000b: ldnull
IL_000c: ldloca V_6
IL_0010: ldnull
IL_0011: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_0016: stloc V_7
IL_001a: ldloc V_7
IL_001e: ldfld System.Boolean SR_PluginLoader._hook_result::abort
IL_0023: brfalse IL_0032
IL_0028: ldloc V_6
IL_002c: unbox.any System.Collections.Generic.List`1<GameData/Summary>
IL_0031: ret
IL_0032: nop
IL_0033: call System.String AutoSaveDirector::SavePath()
IL_0038: stloc.0
IL_0039: ldloc.0
IL_003a: call System.Boolean System.IO.Directory::Exists(System.String)
IL_003f: brtrue IL_004a
IL_0044: newobj System.Void System.Collections.Generic.List`1<GameData/Summary>::.ctor()
IL_0049: ret
IL_004a: nop
IL_004b: ldloc.0
IL_004c: ldstr "*.sav"
IL_0051: call System.String[] System.IO.Directory::GetFiles(System.String,System.String)
IL_0056: stloc.1
IL_0057: newobj System.Void System.Collections.Generic.List`1<GameData/Summary>::.ctor()
IL_005c: stloc.2
IL_005d: ldloc.1
IL_005e: stloc.s V_4
IL_0060: ldc.i4.0
IL_0061: stloc.s V_5
IL_0063: br IL_0086
IL_0068: nop
IL_0069: ldloc.s V_4
IL_006b: ldloc.s V_5
IL_006d: ldelem.ref
IL_006e: stloc.3
IL_006f: ldloc.2
IL_0070: ldloc.3
IL_0071: call System.String System.IO.Path::GetFileNameWithoutExtension(System.String)
IL_0076: call GameData/Summary GameData::LoadSummary(System.String)
IL_007b: callvirt System.Void System.Collections.Generic.List`1<GameData/Summary>::Add(!0)
IL_0080: ldloc.s V_5
IL_0082: ldc.i4.1
IL_0083: add
IL_0084: stloc.s V_5
IL_0086: ldloc.s V_5
IL_0088: ldloc.s V_4
IL_008a: ldlen
IL_008b: conv.i4
IL_008c: blt IL_0068
IL_0091: ldloc.2
IL_0092: ret

*/

// GameData
public static List<GameData.Summary> AvailableGames()
{
	object obj = null;
	_hook_result hook_result = SiscosHooks.call(HOOK_ID.Get_Available_Saves, null, ref obj, null);
	if (hook_result.abort)
	{
		return (List<GameData.Summary>)obj;
	}
	string path = AutoSaveDirector.SavePath();
	if (!Directory.Exists(path))
	{
		return new List<GameData.Summary>();
	}
	string[] files = Directory.GetFiles(path, "*.sav");
	List<GameData.Summary> list = new List<GameData.Summary>();
	string[] array = files;
	for (int i = 0; i < array.Length; i++)
	{
		string path2 = array[i];
		list.Add(GameData.LoadSummary(Path.GetFileNameWithoutExtension(path2)));
	}
	return list;
}

/*
IL_0000: ldnull
IL_0001: stloc V_1
IL_0005: nop
IL_0006: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::Ext_Pre_Game_Loaded
IL_000b: ldnull
IL_000c: ldloca V_1
IL_0010: ldc.i4 2
IL_0015: newarr System.Object
IL_001a: dup
IL_001b: ldc.i4 0
IL_0020: ldarg gameName
IL_0024: box System.String
IL_0029: stelem.ref
IL_002a: dup
IL_002b: ldc.i4 1
IL_0030: ldarg deserialize
IL_0034: box System.Boolean
IL_0039: stelem.ref
IL_003a: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_003f: stloc V_2
IL_0043: ldloc V_2
IL_0047: ldfld System.Boolean SR_PluginLoader._hook_result::abort
IL_004c: brfalse IL_005b
IL_0051: ldloc V_1
IL_0055: unbox.any GameData
IL_005a: ret
IL_005b: nop
IL_005c: ldloc V_2
IL_0060: ldfld System.Object[] SR_PluginLoader._hook_result::args
IL_0065: brfalse IL_0096
IL_006a: ldloc V_2
IL_006e: ldfld System.Object[] SR_PluginLoader._hook_result::args
IL_0073: ldc.i4 0
IL_0078: ldelem.ref
IL_0079: unbox.any System.String
IL_007e: starg.s gameName
IL_0080: ldloc V_2
IL_0084: ldfld System.Object[] SR_PluginLoader._hook_result::args
IL_0089: ldc.i4 1
IL_008e: ldelem.ref
IL_008f: unbox.any System.Boolean
IL_0094: starg.s deserialize
IL_0096: nop
IL_0097: ldarg.0
IL_0098: call System.String GameData::ToPath(System.String)
IL_009d: stloc.0
IL_009e: ldarg.0
IL_009f: ldloc.0
IL_00a0: ldarg.1
IL_00a1: call GameData GameData::LoadPath(System.String,System.String,System.Boolean)
IL_00a6: box GameData
IL_00ab: stloc V_1
IL_00af: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::Ext_Post_Game_Loaded
IL_00b4: ldnull
IL_00b5: ldloca V_1
IL_00b9: ldc.i4 2
IL_00be: newarr System.Object
IL_00c3: dup
IL_00c4: ldc.i4 0
IL_00c9: ldarg gameName
IL_00cd: box System.String
IL_00d2: stelem.ref
IL_00d3: dup
IL_00d4: ldc.i4 1
IL_00d9: ldarg deserialize
IL_00dd: box System.Boolean
IL_00e2: stelem.ref
IL_00e3: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_00e8: stloc V_3
IL_00ec: ldloc V_1
IL_00f0: unbox.any GameData
IL_00f5: ret

*/

// GameData
public static GameData Load(string gameName, bool deserialize = true)
{
	object obj = null;
	_hook_result hook_result = SiscosHooks.call(HOOK_ID.Ext_Pre_Game_Loaded, null, ref obj, new object[]
	{
		gameName,
		deserialize
	});
	if (hook_result.abort)
	{
		return (GameData)obj;
	}
	if (hook_result.args != null)
	{
		gameName = (string)hook_result.args[0];
		deserialize = (bool)hook_result.args[1];
	}
	string fullFilename = GameData.ToPath(gameName);
	obj = GameData.LoadPath(gameName, fullFilename, deserialize);
	_hook_result hook_result2 = SiscosHooks.call(HOOK_ID.Ext_Post_Game_Loaded, null, ref obj, new object[]
	{
		gameName,
		deserialize
	});
	return (GameData)obj;
}

/*
IL_0000: ldnull
IL_0001: stloc V_3
IL_0005: nop
IL_0006: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::Ext_Game_Saved
IL_000b: ldarg 
IL_000f: ldloca V_3
IL_0013: ldnull
IL_0014: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_0019: stloc V_4
IL_001d: ldloc V_4
IL_0021: ldfld System.Boolean SR_PluginLoader._hook_result::abort
IL_0026: brfalse IL_002c
IL_002b: ret
IL_002c: nop
IL_002d: ldarg.0
IL_002e: call System.Void GameData::ToSerializable()
IL_0033: ldarg.0
IL_0034: ldfld System.String GameData::gameName
IL_0039: brfalse IL_004f
IL_003e: ldarg.0
IL_003f: ldfld System.String GameData::gameName
IL_0044: callvirt System.Int32 System.String::get_Length()
IL_0049: ldc.i4.0
IL_004a: bgt IL_0056
IL_004f: nop
IL_0050: newobj System.Void System.ArgumentException::.ctor()
IL_0055: throw
IL_0056: nop
IL_0057: ldarg.0
IL_0058: ldfld System.String GameData::gameName
IL_005d: call System.String GameData::ToPath(System.String)
IL_0062: stloc.0
IL_0063: call System.Runtime.Serialization.Formatters.Binary.BinaryFormatter GameData::CreateFormatter()
IL_0068: stloc.1
IL_0069: ldloc.0
IL_006a: ldstr ".tmp"
IL_006f: call System.String System.String::Concat(System.String,System.String)
IL_0074: call System.IO.FileStream System.IO.File::Create(System.String)
IL_0079: stloc.2
IL_007a: ldloc.1
IL_007b: ldloc.2
IL_007c: ldc.i4.3
IL_007d: box System.Int32
IL_0082: callvirt System.Void System.Runtime.Serialization.Formatters.Binary.BinaryFormatter::Serialize(System.IO.Stream,System.Object)
IL_0087: ldarg.0
IL_0088: ldfld WorldData GameData::world
IL_008d: ldloc.1
IL_008e: ldloc.2
IL_008f: ldc.i4.6
IL_0090: callvirt System.Void DataModule`1<WorldData>::Serialize(System.Runtime.Serialization.Formatters.Binary.BinaryFormatter,System.IO.FileStream,System.Int32)
IL_0095: ldarg.0
IL_0096: ldfld PlayerData GameData::player
IL_009b: ldloc.1
IL_009c: ldloc.2
IL_009d: ldc.i4.3
IL_009e: callvirt System.Void DataModule`1<PlayerData>::Serialize(System.Runtime.Serialization.Formatters.Binary.BinaryFormatter,System.IO.FileStream,System.Int32)
IL_00a3: ldarg.0
IL_00a4: ldfld RanchData GameData::ranch
IL_00a9: ldloc.1
IL_00aa: ldloc.2
IL_00ab: ldc.i4.3
IL_00ac: callvirt System.Void DataModule`1<RanchData>::Serialize(System.Runtime.Serialization.Formatters.Binary.BinaryFormatter,System.IO.FileStream,System.Int32)
IL_00b1: ldarg.0
IL_00b2: ldfld ActorsData GameData::actors
IL_00b7: ldloc.1
IL_00b8: ldloc.2
IL_00b9: ldc.i4.1
IL_00ba: callvirt System.Void DataModule`1<ActorsData>::Serialize(System.Runtime.Serialization.Formatters.Binary.BinaryFormatter,System.IO.FileStream,System.Int32)
IL_00bf: ldarg.0
IL_00c0: ldfld PediaData GameData::pedia
IL_00c5: ldloc.1
IL_00c6: ldloc.2
IL_00c7: ldc.i4.1
IL_00c8: callvirt System.Void DataModule`1<PediaData>::Serialize(System.Runtime.Serialization.Formatters.Binary.BinaryFormatter,System.IO.FileStream,System.Int32)
IL_00cd: ldarg.0
IL_00ce: ldfld GameAchieveData GameData::achieve
IL_00d3: ldloc.1
IL_00d4: ldloc.2
IL_00d5: ldc.i4.1
IL_00d6: callvirt System.Void DataModule`1<GameAchieveData>::Serialize(System.Runtime.Serialization.Formatters.Binary.BinaryFormatter,System.IO.FileStream,System.Int32)
IL_00db: leave IL_00ed
IL_00e0: ldloc.2
IL_00e1: brfalse IL_00ec
IL_00e6: ldloc.2
IL_00e7: callvirt System.Void System.IDisposable::Dispose()
IL_00ec: endfinally
IL_00ed: ldloc.0
IL_00ee: ldstr ".tmp"
IL_00f3: call System.String System.String::Concat(System.String,System.String)
IL_00f8: ldloc.0
IL_00f9: ldc.i4.1
IL_00fa: call System.Void System.IO.File::Copy(System.String,System.String,System.Boolean)
IL_00ff: ldloc.0
IL_0100: ldstr ".tmp"
IL_0105: call System.String System.String::Concat(System.String,System.String)
IL_010a: call System.Void System.IO.File::Delete(System.String)
IL_010f: ret

*/

// GameData
public void Save()
{
	object obj = null;
	_hook_result hook_result = SiscosHooks.call(HOOK_ID.Ext_Game_Saved, this, ref obj, null);
	if (hook_result.abort)
	{
		return;
	}
	this.ToSerializable();
	if (this.gameName == null || this.gameName.Length <= 0)
	{
		throw new ArgumentException();
	}
	string text = GameData.ToPath(this.gameName);
	BinaryFormatter binaryFormatter = GameData.CreateFormatter();
	using (FileStream fileStream = File.Create(text + ".tmp"))
	{
		binaryFormatter.Serialize(fileStream, 3);
		this.world.Serialize(binaryFormatter, fileStream, 6);
		this.player.Serialize(binaryFormatter, fileStream, 3);
		this.ranch.Serialize(binaryFormatter, fileStream, 3);
		this.actors.Serialize(binaryFormatter, fileStream, 1);
		this.pedia.Serialize(binaryFormatter, fileStream, 1);
		this.achieve.Serialize(binaryFormatter, fileStream, 1);
	}
	File.Copy(text + ".tmp", text, true);
	File.Delete(text + ".tmp");
}

/*
IL_0000: ldnull
IL_0001: stloc V_0
IL_0005: nop
IL_0006: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::Get_Save_Directory
IL_000b: ldnull
IL_000c: ldloca V_0
IL_0010: ldc.i4 1
IL_0015: newarr System.Object
IL_001a: dup
IL_001b: ldc.i4 0
IL_0020: ldarg gameName
IL_0024: box System.String
IL_0029: stelem.ref
IL_002a: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_002f: stloc V_1
IL_0033: ldloc V_1
IL_0037: ldfld System.Boolean SR_PluginLoader._hook_result::abort
IL_003c: brfalse IL_004b
IL_0041: ldloc V_0
IL_0045: unbox.any System.String
IL_004a: ret
IL_004b: nop
IL_004c: ldloc V_1
IL_0050: ldfld System.Object[] SR_PluginLoader._hook_result::args
IL_0055: brfalse IL_0070
IL_005a: ldloc V_1
IL_005e: ldfld System.Object[] SR_PluginLoader._hook_result::args
IL_0063: ldc.i4 0
IL_0068: ldelem.ref
IL_0069: unbox.any System.String
IL_006e: starg.s gameName
IL_0070: nop
IL_0071: call System.String AutoSaveDirector::SavePath()
IL_0076: ldarg.0
IL_0077: ldstr ".sav"
IL_007c: call System.String System.String::Concat(System.String,System.String)
IL_0081: call System.String System.IO.Path::Combine(System.String,System.String)
IL_0086: ret

*/

// GameData
public static string ToPath(string gameName)
{
	object obj = null;
	_hook_result hook_result = SiscosHooks.call(HOOK_ID.Get_Save_Directory, null, ref obj, new object[]
	{
		gameName
	});
	if (hook_result.abort)
	{
		return (string)obj;
	}
	if (hook_result.args != null)
	{
		gameName = (string)hook_result.args[0];
	}
	return Path.Combine(AutoSaveDirector.SavePath(), gameName + ".sav");
}

/*
IL_0000: ldnull
IL_0001: stloc V_3
IL_0005: nop
IL_0006: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::Pre_Garden_Init
IL_000b: ldarg 
IL_000f: ldloca V_3
IL_0013: ldnull
IL_0014: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_0019: stloc V_4
IL_001d: ldloc V_4
IL_0021: ldfld System.Boolean SR_PluginLoader._hook_result::abort
IL_0026: brfalse IL_002c
IL_002b: ret
IL_002c: nop
IL_002d: ldarg.0
IL_002e: ldfld GardenCatcher/PlantSlot[] GardenCatcher::plantable
IL_0033: stloc.1
IL_0034: ldc.i4.0
IL_0035: stloc.2
IL_0036: br IL_005b
IL_003b: nop
IL_003c: ldloc.1
IL_003d: ldloc.2
IL_003e: ldelem.ref
IL_003f: stloc.0
IL_0040: ldarg.0
IL_0041: ldfld System.Collections.Generic.Dictionary`2<Identifiable/Id,UnityEngine.GameObject> GardenCatcher::plantableDict
IL_0046: ldloc.0
IL_0047: ldfld Identifiable/Id GardenCatcher/PlantSlot::id
IL_004c: ldloc.0
IL_004d: ldfld UnityEngine.GameObject GardenCatcher/PlantSlot::plantedPrefab
IL_0052: callvirt System.Void System.Collections.Generic.Dictionary`2<Identifiable/Id,UnityEngine.GameObject>::set_Item(!0,!1)
IL_0057: ldloc.2
IL_0058: ldc.i4.1
IL_0059: add
IL_005a: stloc.2
IL_005b: ldloc.2
IL_005c: ldloc.1
IL_005d: ldlen
IL_005e: conv.i4
IL_005f: blt IL_003b
IL_0064: ldarg.0
IL_0065: call T SRSingleton`1<SceneContext>::get_Instance()
IL_006a: callvirt TutorialDirector SceneContext::get_TutorialDirector()
IL_006f: stfld TutorialDirector GardenCatcher::tutDir
IL_0074: nop
IL_0075: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::Post_Garden_Init
IL_007a: ldarg 
IL_007e: ldloca V_3
IL_0082: ldnull
IL_0083: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_0088: pop
IL_0089: ret

*/

// GardenCatcher
public void Awake()
{
	object obj = null;
	_hook_result hook_result = SiscosHooks.call(HOOK_ID.Pre_Garden_Init, this, ref obj, null);
	if (hook_result.abort)
	{
		return;
	}
	GardenCatcher.PlantSlot[] array = this.plantable;
	for (int i = 0; i < array.Length; i++)
	{
		GardenCatcher.PlantSlot plantSlot = array[i];
		this.plantableDict[plantSlot.id] = plantSlot.plantedPrefab;
	}
	this.tutDir = SRSingleton<SceneContext>.Instance.TutorialDirector;
	SiscosHooks.call(HOOK_ID.Post_Garden_Init, this, ref obj, null);
}

/*
IL_0000: ldnull
IL_0001: stloc V_2
IL_0005: nop
IL_0006: ldarg.1
IL_0007: callvirt System.Boolean UnityEngine.Collider::get_isTrigger()
IL_000c: brfalse IL_0012
IL_0011: ret
IL_0012: nop
IL_0013: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::Garden_Got_Input
IL_0018: ldarg 
IL_001c: ldloca V_2
IL_0020: ldc.i4 1
IL_0025: newarr System.Object
IL_002a: dup
IL_002b: ldc.i4 0
IL_0030: ldarg col
IL_0034: box UnityEngine.Collider
IL_0039: stelem.ref
IL_003a: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_003f: stloc V_3
IL_0043: ldloc V_3
IL_0047: ldfld System.Boolean SR_PluginLoader._hook_result::abort
IL_004c: brfalse IL_0052
IL_0051: ret
IL_0052: nop
IL_0053: ldloc V_3
IL_0057: ldfld System.Object[] SR_PluginLoader._hook_result::args
IL_005c: brfalse IL_0077
IL_0061: ldloc V_3
IL_0065: ldfld System.Object[] SR_PluginLoader._hook_result::args
IL_006a: ldc.i4 0
IL_006f: ldelem.ref
IL_0070: unbox.any UnityEngine.Collider
IL_0075: starg.s col
IL_0077: nop
IL_0078: ldarg.0
IL_0079: ldfld LandPlot GardenCatcher::activator
IL_007e: callvirt System.Boolean LandPlot::HasAttached()
IL_0083: brfalse IL_0089
IL_0088: ret
IL_0089: nop
IL_008a: ldarg.1
IL_008b: callvirt !!0 UnityEngine.Component::GetComponent<Identifiable>()
IL_0090: stloc.0
IL_0091: ldloc.0
IL_0092: ldnull
IL_0093: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_0098: brfalse IL_01de
IL_009d: ldarg.0
IL_009e: ldfld System.Collections.Generic.Dictionary`2<Identifiable/Id,UnityEngine.GameObject> GardenCatcher::plantableDict
IL_00a3: ldloc.0
IL_00a4: ldfld Identifiable/Id Identifiable::id
IL_00a9: callvirt System.Boolean System.Collections.Generic.Dictionary`2<Identifiable/Id,UnityEngine.GameObject>::ContainsKey(!0)
IL_00ae: brfalse IL_01de
IL_00b3: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::Pre_Garden_Set_Type
IL_00b8: ldarg 
IL_00bc: ldloca V_2
IL_00c0: ldc.i4 1
IL_00c5: newarr System.Object
IL_00ca: dup
IL_00cb: ldc.i4 0
IL_00d0: ldarg col
IL_00d4: box UnityEngine.Collider
IL_00d9: stelem.ref
IL_00da: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_00df: stloc V_4
IL_00e3: ldloc V_4
IL_00e7: ldfld System.Boolean SR_PluginLoader._hook_result::abort
IL_00ec: brfalse IL_00f2
IL_00f1: ret
IL_00f2: nop
IL_00f3: ldloc V_4
IL_00f7: ldfld System.Object[] SR_PluginLoader._hook_result::args
IL_00fc: brfalse IL_0117
IL_0101: ldloc V_4
IL_0105: ldfld System.Object[] SR_PluginLoader._hook_result::args
IL_010a: ldc.i4 0
IL_010f: ldelem.ref
IL_0110: unbox.any UnityEngine.Collider
IL_0115: starg.s col
IL_0117: nop
IL_0118: ldarg.0
IL_0119: ldfld System.Collections.Generic.Dictionary`2<Identifiable/Id,UnityEngine.GameObject> GardenCatcher::plantableDict
IL_011e: ldloc.0
IL_011f: ldfld Identifiable/Id Identifiable::id
IL_0124: callvirt !1 System.Collections.Generic.Dictionary`2<Identifiable/Id,UnityEngine.GameObject>::get_Item(!0)
IL_0129: ldarg.0
IL_012a: ldfld LandPlot GardenCatcher::activator
IL_012f: callvirt UnityEngine.Transform UnityEngine.Component::get_transform()
IL_0134: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_position()
IL_0139: ldarg.0
IL_013a: ldfld LandPlot GardenCatcher::activator
IL_013f: callvirt UnityEngine.Transform UnityEngine.Component::get_transform()
IL_0144: callvirt UnityEngine.Quaternion UnityEngine.Transform::get_rotation()
IL_0149: call UnityEngine.Object UnityEngine.Object::Instantiate(UnityEngine.Object,UnityEngine.Vector3,UnityEngine.Quaternion)
IL_014e: isinst UnityEngine.GameObject
IL_0153: stloc.1
IL_0154: ldloc.1
IL_0155: callvirt !!0 UnityEngine.GameObject::AddComponent<DestroyAfterTime>()
IL_015a: pop
IL_015b: ldarg.0
IL_015c: ldfld LandPlot GardenCatcher::activator
IL_0161: ldloc.1
IL_0162: callvirt System.Void LandPlot::Attach(UnityEngine.GameObject)
IL_0167: ldarg.0
IL_0168: ldfld TutorialDirector GardenCatcher::tutDir
IL_016d: callvirt System.Void TutorialDirector::OnPlanted()
IL_0172: ldarg.0
IL_0173: ldfld UnityEngine.GameObject GardenCatcher::acceptFX
IL_0178: ldnull
IL_0179: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_017e: brfalse IL_01a5
IL_0183: ldarg.0
IL_0184: ldfld UnityEngine.GameObject GardenCatcher::acceptFX
IL_0189: ldarg.0
IL_018a: call UnityEngine.Transform UnityEngine.Component::get_transform()
IL_018f: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_position()
IL_0194: ldarg.0
IL_0195: call UnityEngine.Transform UnityEngine.Component::get_transform()
IL_019a: callvirt UnityEngine.Quaternion UnityEngine.Transform::get_rotation()
IL_019f: call UnityEngine.GameObject SRBehaviour::InstantiateDynamic(UnityEngine.GameObject,UnityEngine.Vector3,UnityEngine.Quaternion)
IL_01a4: pop
IL_01a5: nop
IL_01a6: ldarg.1
IL_01a7: callvirt UnityEngine.GameObject UnityEngine.Component::get_gameObject()
IL_01ac: call System.Void UnityEngine.Object::Destroy(UnityEngine.Object)
IL_01b1: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::Post_Garden_Set_Type
IL_01b6: ldarg 
IL_01ba: ldloca V_2
IL_01be: ldc.i4 1
IL_01c3: newarr System.Object
IL_01c8: dup
IL_01c9: ldc.i4 0
IL_01ce: ldarg col
IL_01d2: box UnityEngine.Collider
IL_01d7: stelem.ref
IL_01d8: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_01dd: pop
IL_01de: nop
IL_01df: ret

*/

// GardenCatcher
public void OnTriggerEnter(Collider col)
{
	object obj = null;
	if (col.isTrigger)
	{
		return;
	}
	_hook_result hook_result = SiscosHooks.call(HOOK_ID.Garden_Got_Input, this, ref obj, new object[]
	{
		col
	});
	if (hook_result.abort)
	{
		return;
	}
	if (hook_result.args != null)
	{
		col = (Collider)hook_result.args[0];
	}
	if (this.activator.HasAttached())
	{
		return;
	}
	Identifiable component = col.GetComponent<Identifiable>();
	if (component != null && this.plantableDict.ContainsKey(component.id))
	{
		_hook_result hook_result2 = SiscosHooks.call(HOOK_ID.Pre_Garden_Set_Type, this, ref obj, new object[]
		{
			col
		});
		if (hook_result2.abort)
		{
			return;
		}
		if (hook_result2.args != null)
		{
			col = (Collider)hook_result2.args[0];
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(this.plantableDict[component.id], this.activator.transform.position, this.activator.transform.rotation) as GameObject;
		gameObject.AddComponent<DestroyAfterTime>();
		this.activator.Attach(gameObject);
		this.tutDir.OnPlanted();
		if (this.acceptFX != null)
		{
			SRBehaviour.InstantiateDynamic(this.acceptFX, base.transform.position, base.transform.rotation);
		}
		UnityEngine.Object.Destroy(col.gameObject);
		SiscosHooks.call(HOOK_ID.Post_Garden_Set_Type, this, ref obj, new object[]
		{
			col
		});
	}
}

/*
IL_0000: ldnull
IL_0001: stloc V_3
IL_0005: nop
IL_0006: ldc.i4.5
IL_0007: newarr PurchaseUI/Purchasable
IL_000c: dup
IL_000d: ldc.i4.0
IL_000e: ldstr "m.upgrade.name.garden.soil"
IL_0013: ldarg.0
IL_0014: ldfld LandPlotUI/UpgradePurchaseItem GardenUI::soil
IL_0019: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_001e: ldarg.0
IL_001f: ldfld LandPlotUI/UpgradePurchaseItem GardenUI::soil
IL_0024: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_0029: ldstr "m.upgrade.desc.garden.soil"
IL_002e: ldarg.0
IL_002f: ldfld LandPlotUI/UpgradePurchaseItem GardenUI::soil
IL_0034: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_0039: ldc.i4 4002
IL_003e: newobj System.Void System.Nullable`1<PediaDirector/Id>::.ctor(!0)
IL_0043: ldarg.0
IL_0044: ldftn System.Void GardenUI::UpgradeSoil()
IL_004a: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_004f: ldarg.0
IL_0050: ldfld LandPlot LandPlotUI::activator
IL_0055: ldc.i4.6
IL_0056: callvirt System.Boolean LandPlot::HasUpgrade(LandPlot/Upgrade)
IL_005b: ldc.i4.0
IL_005c: ceq
IL_005e: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_0063: stelem.ref
IL_0064: dup
IL_0065: ldc.i4.1
IL_0066: ldstr "m.upgrade.name.garden.sprinkler"
IL_006b: ldarg.0
IL_006c: ldfld LandPlotUI/UpgradePurchaseItem GardenUI::sprinkler
IL_0071: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_0076: ldarg.0
IL_0077: ldfld LandPlotUI/UpgradePurchaseItem GardenUI::sprinkler
IL_007c: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_0081: ldstr "m.upgrade.desc.garden.sprinkler"
IL_0086: ldarg.0
IL_0087: ldfld LandPlotUI/UpgradePurchaseItem GardenUI::sprinkler
IL_008c: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_0091: ldc.i4 4002
IL_0096: newobj System.Void System.Nullable`1<PediaDirector/Id>::.ctor(!0)
IL_009b: ldarg.0
IL_009c: ldftn System.Void GardenUI::UpgradeSprinkler()
IL_00a2: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_00a7: ldarg.0
IL_00a8: ldfld LandPlot LandPlotUI::activator
IL_00ad: ldc.i4.7
IL_00ae: callvirt System.Boolean LandPlot::HasUpgrade(LandPlot/Upgrade)
IL_00b3: ldc.i4.0
IL_00b4: ceq
IL_00b6: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_00bb: stelem.ref
IL_00bc: dup
IL_00bd: ldc.i4.2
IL_00be: ldstr "m.upgrade.name.garden.scareslime"
IL_00c3: ldarg.0
IL_00c4: ldfld LandPlotUI/UpgradePurchaseItem GardenUI::scareslime
IL_00c9: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_00ce: ldarg.0
IL_00cf: ldfld LandPlotUI/UpgradePurchaseItem GardenUI::scareslime
IL_00d4: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_00d9: ldstr "m.upgrade.desc.garden.scareslime"
IL_00de: ldarg.0
IL_00df: ldfld LandPlotUI/UpgradePurchaseItem GardenUI::scareslime
IL_00e4: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_00e9: ldc.i4 4002
IL_00ee: newobj System.Void System.Nullable`1<PediaDirector/Id>::.ctor(!0)
IL_00f3: ldarg.0
IL_00f4: ldftn System.Void GardenUI::UpgradeScareslime()
IL_00fa: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_00ff: ldarg.0
IL_0100: ldfld LandPlot LandPlotUI::activator
IL_0105: ldc.i4.8
IL_0106: callvirt System.Boolean LandPlot::HasUpgrade(LandPlot/Upgrade)
IL_010b: ldc.i4.0
IL_010c: ceq
IL_010e: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_0113: stelem.ref
IL_0114: dup
IL_0115: ldc.i4.3
IL_0116: ldstr "ui"
IL_011b: ldstr "b.clear_crop"
IL_0120: call System.String MessageUtil::Qualify(System.String,System.String)
IL_0125: ldarg.0
IL_0126: ldfld LandPlotUI/PurchaseItem GardenUI::clearCrop
IL_012b: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_0130: ldarg.0
IL_0131: ldfld LandPlotUI/PurchaseItem GardenUI::clearCrop
IL_0136: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_013b: ldstr "ui"
IL_0140: ldstr "m.desc.clear_crop"
IL_0145: call System.String MessageUtil::Qualify(System.String,System.String)
IL_014a: ldarg.0
IL_014b: ldfld LandPlotUI/PurchaseItem GardenUI::clearCrop
IL_0150: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_0155: ldloca.s V_1
IL_0157: initobj System.Nullable`1<PediaDirector/Id>
IL_015d: ldloc.1
IL_015e: ldarg.0
IL_015f: ldftn System.Void GardenUI::ClearCrop()
IL_0165: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_016a: ldarg.0
IL_016b: ldfld LandPlot LandPlotUI::activator
IL_0170: callvirt System.Boolean LandPlot::HasAttached()
IL_0175: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_017a: stelem.ref
IL_017b: dup
IL_017c: ldc.i4.4
IL_017d: ldstr "ui"
IL_0182: ldstr "b.demolish"
IL_0187: call System.String MessageUtil::Qualify(System.String,System.String)
IL_018c: ldarg.0
IL_018d: ldfld LandPlotUI/PlotPurchaseItem GardenUI::demolish
IL_0192: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_0197: ldarg.0
IL_0198: ldfld LandPlotUI/PlotPurchaseItem GardenUI::demolish
IL_019d: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_01a2: ldstr "ui"
IL_01a7: ldstr "m.desc.demolish"
IL_01ac: call System.String MessageUtil::Qualify(System.String,System.String)
IL_01b1: ldarg.0
IL_01b2: ldfld LandPlotUI/PlotPurchaseItem GardenUI::demolish
IL_01b7: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_01bc: ldloca.s V_2
IL_01be: initobj System.Nullable`1<PediaDirector/Id>
IL_01c4: ldloc.2
IL_01c5: ldarg.0
IL_01c6: ldftn System.Void GardenUI::Demolish()
IL_01cc: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_01d1: ldc.i4.1
IL_01d2: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_01d7: stelem.ref
IL_01d8: stloc.0
IL_01d9: call T SRSingleton`1<GameContext>::get_Instance()
IL_01de: callvirt UITemplates GameContext::get_UITemplates()
IL_01e3: ldarg.0
IL_01e4: ldfld UnityEngine.Sprite GardenUI::titleIcon
IL_01e9: ldstr "t.garden"
IL_01ee: ldloc.0
IL_01ef: ldarg.0
IL_01f0: dup
IL_01f1: ldvirtftn System.Void BaseUI::Close()
IL_01f7: newobj System.Void PurchaseUI/OnClose::.ctor(System.Object,System.IntPtr)
IL_01fc: callvirt UnityEngine.GameObject UITemplates::CreatePurchaseUI(UnityEngine.Sprite,System.String,PurchaseUI/Purchasable[],PurchaseUI/OnClose)
IL_0201: box UnityEngine.GameObject
IL_0206: stloc V_3
IL_020a: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::Ext_Spawn_Plot_Upgrades_UI
IL_020f: ldarg 
IL_0213: ldloca V_3
IL_0217: ldnull
IL_0218: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_021d: stloc V_4
IL_0221: ldloc V_3
IL_0225: unbox.any UnityEngine.GameObject
IL_022a: ret

*/

// GardenUI
protected override GameObject CreatePurchaseUI()
{
	object obj = null;
	PurchaseUI.Purchasable[] purchasables = new PurchaseUI.Purchasable[]
	{
		new PurchaseUI.Purchasable("m.upgrade.name.garden.soil", this.soil.icon, this.soil.img, "m.upgrade.desc.garden.soil", this.soil.cost, new PediaDirector.Id?(PediaDirector.Id.GARDEN), new UnityAction(this.UpgradeSoil), !this.activator.HasUpgrade(LandPlot.Upgrade.SOIL)),
		new PurchaseUI.Purchasable("m.upgrade.name.garden.sprinkler", this.sprinkler.icon, this.sprinkler.img, "m.upgrade.desc.garden.sprinkler", this.sprinkler.cost, new PediaDirector.Id?(PediaDirector.Id.GARDEN), new UnityAction(this.UpgradeSprinkler), !this.activator.HasUpgrade(LandPlot.Upgrade.SPRINKLER)),
		new PurchaseUI.Purchasable("m.upgrade.name.garden.scareslime", this.scareslime.icon, this.scareslime.img, "m.upgrade.desc.garden.scareslime", this.scareslime.cost, new PediaDirector.Id?(PediaDirector.Id.GARDEN), new UnityAction(this.UpgradeScareslime), !this.activator.HasUpgrade(LandPlot.Upgrade.SCARESLIME)),
		new PurchaseUI.Purchasable(MessageUtil.Qualify("ui", "b.clear_crop"), this.clearCrop.icon, this.clearCrop.img, MessageUtil.Qualify("ui", "m.desc.clear_crop"), this.clearCrop.cost, null, new UnityAction(this.ClearCrop), this.activator.HasAttached()),
		new PurchaseUI.Purchasable(MessageUtil.Qualify("ui", "b.demolish"), this.demolish.icon, this.demolish.img, MessageUtil.Qualify("ui", "m.desc.demolish"), this.demolish.cost, null, new UnityAction(this.Demolish), true)
	};
	obj = SRSingleton<GameContext>.Instance.UITemplates.CreatePurchaseUI(this.titleIcon, "t.garden", purchasables, new PurchaseUI.OnClose(this.Close));
	_hook_result hook_result = SiscosHooks.call(HOOK_ID.Ext_Spawn_Plot_Upgrades_UI, this, ref obj, null);
	return (GameObject)obj;
}

/*
IL_0000: ldnull
IL_0001: stloc V_2
IL_0005: nop
IL_0006: ldc.i4.1
IL_0007: newarr PurchaseUI/Purchasable
IL_000c: dup
IL_000d: ldc.i4.0
IL_000e: ldstr "ui"
IL_0013: ldstr "b.demolish"
IL_0018: call System.String MessageUtil::Qualify(System.String,System.String)
IL_001d: ldarg.0
IL_001e: ldfld LandPlotUI/PlotPurchaseItem IncineratorUI::demolish
IL_0023: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_0028: ldarg.0
IL_0029: ldfld LandPlotUI/PlotPurchaseItem IncineratorUI::demolish
IL_002e: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_0033: ldstr "ui"
IL_0038: ldstr "m.desc.demolish"
IL_003d: call System.String MessageUtil::Qualify(System.String,System.String)
IL_0042: ldarg.0
IL_0043: ldfld LandPlotUI/PlotPurchaseItem IncineratorUI::demolish
IL_0048: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_004d: ldloca.s V_1
IL_004f: initobj System.Nullable`1<PediaDirector/Id>
IL_0055: ldloc.1
IL_0056: ldarg.0
IL_0057: ldftn System.Void IncineratorUI::Demolish()
IL_005d: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_0062: ldc.i4.1
IL_0063: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_0068: stelem.ref
IL_0069: stloc.0
IL_006a: call T SRSingleton`1<GameContext>::get_Instance()
IL_006f: callvirt UITemplates GameContext::get_UITemplates()
IL_0074: ldarg.0
IL_0075: ldfld UnityEngine.Sprite IncineratorUI::titleIcon
IL_007a: ldstr "t.incinerator"
IL_007f: ldloc.0
IL_0080: ldarg.0
IL_0081: dup
IL_0082: ldvirtftn System.Void BaseUI::Close()
IL_0088: newobj System.Void PurchaseUI/OnClose::.ctor(System.Object,System.IntPtr)
IL_008d: callvirt UnityEngine.GameObject UITemplates::CreatePurchaseUI(UnityEngine.Sprite,System.String,PurchaseUI/Purchasable[],PurchaseUI/OnClose)
IL_0092: box UnityEngine.GameObject
IL_0097: stloc V_2
IL_009b: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::Ext_Spawn_Plot_Upgrades_UI
IL_00a0: ldarg 
IL_00a4: ldloca V_2
IL_00a8: ldnull
IL_00a9: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_00ae: stloc V_3
IL_00b2: ldloc V_2
IL_00b6: unbox.any UnityEngine.GameObject
IL_00bb: ret

*/

// IncineratorUI
protected override GameObject CreatePurchaseUI()
{
	object obj = null;
	PurchaseUI.Purchasable[] purchasables = new PurchaseUI.Purchasable[]
	{
		new PurchaseUI.Purchasable(MessageUtil.Qualify("ui", "b.demolish"), this.demolish.icon, this.demolish.img, MessageUtil.Qualify("ui", "m.desc.demolish"), this.demolish.cost, null, new UnityAction(this.Demolish), true)
	};
	obj = SRSingleton<GameContext>.Instance.UITemplates.CreatePurchaseUI(this.titleIcon, "t.incinerator", purchasables, new PurchaseUI.OnClose(this.Close));
	_hook_result hook_result = SiscosHooks.call(HOOK_ID.Ext_Spawn_Plot_Upgrades_UI, this, ref obj, null);
	return (GameObject)obj;
}

/*
IL_0000: ldnull
IL_0001: stloc V_2
IL_0005: nop
IL_0006: ldarg.0
IL_0007: ldarg.1
IL_0008: stfld System.Collections.Generic.List`1<LandPlot/Upgrade> LandPlot::upgrades
IL_000d: ldarg.1
IL_000e: callvirt System.Collections.Generic.List`1/Enumerator<!0> System.Collections.Generic.List`1<LandPlot/Upgrade>::GetEnumerator()
IL_0013: stloc.1
IL_0014: br IL_0029
IL_0019: nop
IL_001a: ldloca.s V_1
IL_001c: call !0 System.Collections.Generic.List`1/Enumerator<LandPlot/Upgrade>::get_Current()
IL_0021: stloc.0
IL_0022: ldarg.0
IL_0023: ldloc.0
IL_0024: call System.Void LandPlot::ApplyUpgrade(LandPlot/Upgrade)
IL_0029: ldloca.s V_1
IL_002b: call System.Boolean System.Collections.Generic.List`1/Enumerator<LandPlot/Upgrade>::MoveNext()
IL_0030: brtrue IL_0019
IL_0035: leave IL_0046
IL_003a: ldloc.1
IL_003b: box System.Collections.Generic.List`1/Enumerator<LandPlot/Upgrade>
IL_0040: callvirt System.Void System.IDisposable::Dispose()
IL_0045: endfinally
IL_0046: nop
IL_0047: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::Plot_Load_Upgrades
IL_004c: ldarg 
IL_0050: ldloca V_2
IL_0054: ldc.i4 1
IL_0059: newarr System.Object
IL_005e: dup
IL_005f: ldc.i4 0
IL_0064: ldarg upgrades
IL_0068: box System.Collections.Generic.List`1<LandPlot/Upgrade>
IL_006d: stelem.ref
IL_006e: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_0073: pop
IL_0074: ret

*/

// LandPlot
public void SetUpgrades(List<LandPlot.Upgrade> upgrades)
{
	object obj = null;
	this.upgrades = upgrades;
	foreach (LandPlot.Upgrade current in upgrades)
	{
		this.ApplyUpgrade(current);
	}
	SiscosHooks.call(HOOK_ID.Plot_Load_Upgrades, this, ref obj, new object[]
	{
		upgrades
	});
}

/*
IL_0000: ldnull
IL_0001: stloc V_0
IL_0005: nop
IL_0006: ldarg.0
IL_0007: ldfld AchievementsDirector LockOnDeath::achieveDir
IL_000c: ldc.i4.2
IL_000d: ldarg.0
IL_000e: ldfld TimeDirector LockOnDeath::timeDir
IL_0013: callvirt System.Single TimeDirector::WorldTime()
IL_0018: callvirt System.Void AchievementsDirector::SetStat(AchievementsDirector/GameFloatStat,System.Single)
IL_001d: ldarg.0
IL_001e: ldarg.1
IL_001f: stfld System.Single LockOnDeath::unlockWorldTime
IL_0024: ldarg.0
IL_0025: ldc.i4.1
IL_0026: stfld System.Boolean LockOnDeath::locked
IL_002b: ldarg.0
IL_002c: call System.Void LockOnDeath::Freeze()
IL_0031: ldarg.0
IL_0032: ldfld LockOnDeath/OnLockChanged LockOnDeath::onLockChanged
IL_0037: brfalse IL_0048
IL_003c: ldarg.0
IL_003d: ldfld LockOnDeath/OnLockChanged LockOnDeath::onLockChanged
IL_0042: ldc.i4.1
IL_0043: callvirt System.Void LockOnDeath/OnLockChanged::Invoke(System.Boolean)
IL_0048: nop
IL_0049: ldarg.0
IL_004a: ldarg.3
IL_004b: stfld UnityEngine.Events.UnityAction LockOnDeath::onLockComplete
IL_0050: ldarg.0
IL_0051: ldarg.0
IL_0052: ldarg.2
IL_0053: call System.Collections.IEnumerator LockOnDeath::DelayedFastForward(System.Single)
IL_0058: call UnityEngine.Coroutine UnityEngine.MonoBehaviour::StartCoroutine(System.Collections.IEnumerator)
IL_005d: pop
IL_005e: nop
IL_005f: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::Pre_Player_Sleep
IL_0064: ldarg 
IL_0068: ldloca V_0
IL_006c: ldc.i4 3
IL_0071: newarr System.Object
IL_0076: dup
IL_0077: ldc.i4 0
IL_007c: ldarg unlockWorldTime
IL_0080: box System.Single
IL_0085: stelem.ref
IL_0086: dup
IL_0087: ldc.i4 1
IL_008c: ldarg delayTime
IL_0090: box System.Single
IL_0095: stelem.ref
IL_0096: dup
IL_0097: ldc.i4 2
IL_009c: ldarg onComplete
IL_00a0: box UnityEngine.Events.UnityAction
IL_00a5: stelem.ref
IL_00a6: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_00ab: pop
IL_00ac: ret

*/

// LockOnDeath
public void LockUntil(float unlockWorldTime, float delayTime, UnityAction onComplete = null)
{
	object obj = null;
	this.achieveDir.SetStat(AchievementsDirector.GameFloatStat.LAST_SLEPT, this.timeDir.WorldTime());
	this.unlockWorldTime = unlockWorldTime;
	this.locked = true;
	this.Freeze();
	if (this.onLockChanged != null)
	{
		this.onLockChanged(true);
	}
	this.onLockComplete = onComplete;
	base.StartCoroutine(this.DelayedFastForward(delayTime));
	SiscosHooks.call(HOOK_ID.Pre_Player_Sleep, this, ref obj, new object[]
	{
		unlockWorldTime,
		delayTime,
		onComplete
	});
}

/*
IL_0000: ldnull
IL_0001: stloc V_1
IL_0005: nop
IL_0006: ldarg.0
IL_0007: ldfld System.Boolean LockOnDeath::locked
IL_000c: brfalse IL_00df
IL_0011: ldarg.0
IL_0012: ldfld TimeDirector LockOnDeath::timeDir
IL_0017: ldarg.0
IL_0018: ldfld System.Single LockOnDeath::unlockWorldTime
IL_001d: callvirt System.Boolean TimeDirector::HasReached(System.Single)
IL_0022: brfalse IL_00df
IL_0027: ldarg.0
IL_0028: ldc.i4.0
IL_0029: stfld System.Boolean LockOnDeath::locked
IL_002e: ldarg.0
IL_002f: call System.Void LockOnDeath::Unfreeze()
IL_0034: ldarg.0
IL_0035: ldfld LockOnDeath/OnLockChanged LockOnDeath::onLockChanged
IL_003a: brfalse IL_004b
IL_003f: ldarg.0
IL_0040: ldfld LockOnDeath/OnLockChanged LockOnDeath::onLockChanged
IL_0045: ldc.i4.0
IL_0046: callvirt System.Void LockOnDeath/OnLockChanged::Invoke(System.Boolean)
IL_004b: nop
IL_004c: ldarg.0
IL_004d: ldfld TimeDirector LockOnDeath::timeDir
IL_0052: ldc.i4.0
IL_0053: callvirt System.Void TimeDirector::SetFastForward(System.Boolean)
IL_0058: ldarg.0
IL_0059: ldflda SECTR_AudioCueInstance LockOnDeath::timePassingInstance
IL_005e: ldc.i4.0
IL_005f: call System.Void SECTR_AudioCueInstance::Stop(System.Boolean)
IL_0064: ldarg.0
IL_0065: ldfld SECTR_AudioCue LockOnDeath::playerWakeCue
IL_006a: ldnull
IL_006b: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_0070: brfalse IL_008e
IL_0075: ldarg.0
IL_0076: call !!0 UnityEngine.Component::GetComponent<SECTR_AudioSource>()
IL_007b: stloc.0
IL_007c: ldloc.0
IL_007d: ldarg.0
IL_007e: ldfld SECTR_AudioCue LockOnDeath::playerWakeCue
IL_0083: stfld SECTR_AudioCue SECTR_AudioSource::Cue
IL_0088: ldloc.0
IL_0089: callvirt System.Void SECTR_AudioSource::Play()
IL_008e: nop
IL_008f: call T SRSingleton`1<GameContext>::get_Instance()
IL_0094: callvirt AutoSaveDirector GameContext::get_AutoSaveDirector()
IL_0099: callvirt System.Void AutoSaveDirector::SaveGame()
IL_009e: ldarg.0
IL_009f: ldfld AchievementsDirector LockOnDeath::achieveDir
IL_00a4: ldc.i4.3
IL_00a5: ldarg.0
IL_00a6: ldfld TimeDirector LockOnDeath::timeDir
IL_00ab: callvirt System.Single TimeDirector::WorldTime()
IL_00b0: callvirt System.Void AchievementsDirector::SetStat(AchievementsDirector/GameFloatStat,System.Single)
IL_00b5: ldarg.0
IL_00b6: ldfld UnityEngine.Events.UnityAction LockOnDeath::onLockComplete
IL_00bb: brfalse IL_00df
IL_00c0: ldarg.0
IL_00c1: ldfld UnityEngine.Events.UnityAction LockOnDeath::onLockComplete
IL_00c6: callvirt System.Void UnityEngine.Events.UnityAction::Invoke()
IL_00cb: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::Post_Player_Sleep
IL_00d0: ldarg 
IL_00d4: ldloca V_1
IL_00d8: ldnull
IL_00d9: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_00de: pop
IL_00df: nop
IL_00e0: ret

*/

// LockOnDeath
public void Update()
{
	object obj = null;
	if (this.locked && this.timeDir.HasReached(this.unlockWorldTime))
	{
		this.locked = false;
		this.Unfreeze();
		if (this.onLockChanged != null)
		{
			this.onLockChanged(false);
		}
		this.timeDir.SetFastForward(false);
		this.timePassingInstance.Stop(false);
		if (this.playerWakeCue != null)
		{
			SECTR_AudioSource component = base.GetComponent<SECTR_AudioSource>();
			component.Cue = this.playerWakeCue;
			component.Play();
		}
		SRSingleton<GameContext>.Instance.AutoSaveDirector.SaveGame();
		this.achieveDir.SetStat(AchievementsDirector.GameFloatStat.LAST_AWOKE, this.timeDir.WorldTime());
		if (this.onLockComplete != null)
		{
			this.onLockComplete();
			SiscosHooks.call(HOOK_ID.Post_Player_Sleep, this, ref obj, null);
		}
	}
}

/*
IL_0000: ldnull
IL_0001: stloc V_15
IL_0005: nop
IL_0006: ldc.i4.s 14
IL_0008: newarr PurchaseUI/Purchasable
IL_000d: dup
IL_000e: ldc.i4.0
IL_000f: ldstr "m.upgrade.name.personal.liquid_slot"
IL_0014: ldarg.0
IL_0015: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::liquidSlot
IL_001a: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::icon
IL_001f: ldarg.0
IL_0020: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::liquidSlot
IL_0025: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::img
IL_002a: ldstr "m.upgrade.desc.personal.liquid_slot"
IL_002f: ldarg.0
IL_0030: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::liquidSlot
IL_0035: ldfld System.Int32 PersonalUpgradeUI/UpgradePurchaseItem::cost
IL_003a: ldloca.s V_1
IL_003c: initobj System.Nullable`1<PediaDirector/Id>
IL_0042: ldloc.1
IL_0043: ldarg.0
IL_0044: ldftn System.Void PersonalUpgradeUI::UpgradeLiquidSlot()
IL_004a: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_004f: ldarg.0
IL_0050: ldfld PlayerState PersonalUpgradeUI::playerState
IL_0055: ldc.i4.s 13
IL_0057: callvirt System.Boolean PlayerState::CanGetUpgrade(PlayerState/Upgrade)
IL_005c: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_0061: stelem.ref
IL_0062: dup
IL_0063: ldc.i4.1
IL_0064: ldstr "m.upgrade.name.personal.jetpack"
IL_0069: ldarg.0
IL_006a: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::jetpack
IL_006f: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::icon
IL_0074: ldarg.0
IL_0075: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::jetpack
IL_007a: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::img
IL_007f: ldstr "m.upgrade.desc.personal.jetpack"
IL_0084: ldarg.0
IL_0085: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::jetpack
IL_008a: ldfld System.Int32 PersonalUpgradeUI/UpgradePurchaseItem::cost
IL_008f: ldloca.s V_2
IL_0091: initobj System.Nullable`1<PediaDirector/Id>
IL_0097: ldloc.2
IL_0098: ldarg.0
IL_0099: ldftn System.Void PersonalUpgradeUI::UpgradeJetpack()
IL_009f: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_00a4: ldarg.0
IL_00a5: ldfld PlayerState PersonalUpgradeUI::playerState
IL_00aa: ldc.i4.s 9
IL_00ac: callvirt System.Boolean PlayerState::CanGetUpgrade(PlayerState/Upgrade)
IL_00b1: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_00b6: stelem.ref
IL_00b7: dup
IL_00b8: ldc.i4.2
IL_00b9: ldstr "m.upgrade.name.personal.jetpack_efficiency"
IL_00be: ldarg.0
IL_00bf: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::jetpackEfficiency
IL_00c4: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::icon
IL_00c9: ldarg.0
IL_00ca: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::jetpackEfficiency
IL_00cf: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::img
IL_00d4: ldstr "m.upgrade.desc.personal.jetpack_efficiency"
IL_00d9: ldarg.0
IL_00da: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::jetpackEfficiency
IL_00df: ldfld System.Int32 PersonalUpgradeUI/UpgradePurchaseItem::cost
IL_00e4: ldloca.s V_3
IL_00e6: initobj System.Nullable`1<PediaDirector/Id>
IL_00ec: ldloc.3
IL_00ed: ldarg.0
IL_00ee: ldftn System.Void PersonalUpgradeUI::UpgradeJetpackEfficiency()
IL_00f4: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_00f9: ldarg.0
IL_00fa: ldfld PlayerState PersonalUpgradeUI::playerState
IL_00ff: ldc.i4.s 10
IL_0101: callvirt System.Boolean PlayerState::CanGetUpgrade(PlayerState/Upgrade)
IL_0106: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_010b: stelem.ref
IL_010c: dup
IL_010d: ldc.i4.3
IL_010e: ldstr "m.upgrade.name.personal.run_efficiency"
IL_0113: ldarg.0
IL_0114: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::runEfficiency
IL_0119: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::icon
IL_011e: ldarg.0
IL_011f: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::runEfficiency
IL_0124: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::img
IL_0129: ldstr "m.upgrade.desc.personal.run_efficiency"
IL_012e: ldarg.0
IL_012f: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::runEfficiency
IL_0134: ldfld System.Int32 PersonalUpgradeUI/UpgradePurchaseItem::cost
IL_0139: ldloca.s V_4
IL_013b: initobj System.Nullable`1<PediaDirector/Id>
IL_0141: ldloc.s V_4
IL_0143: ldarg.0
IL_0144: ldftn System.Void PersonalUpgradeUI::UpgradeRunEfficiency()
IL_014a: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_014f: ldarg.0
IL_0150: ldfld PlayerState PersonalUpgradeUI::playerState
IL_0155: ldc.i4.s 12
IL_0157: callvirt System.Boolean PlayerState::CanGetUpgrade(PlayerState/Upgrade)
IL_015c: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_0161: stelem.ref
IL_0162: dup
IL_0163: ldc.i4.4
IL_0164: ldstr "m.upgrade.name.personal.air_burst"
IL_0169: ldarg.0
IL_016a: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::airBurst
IL_016f: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::icon
IL_0174: ldarg.0
IL_0175: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::airBurst
IL_017a: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::img
IL_017f: ldstr "m.upgrade.desc.personal.air_burst"
IL_0184: ldarg.0
IL_0185: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::airBurst
IL_018a: ldfld System.Int32 PersonalUpgradeUI/UpgradePurchaseItem::cost
IL_018f: ldloca.s V_5
IL_0191: initobj System.Nullable`1<PediaDirector/Id>
IL_0197: ldloc.s V_5
IL_0199: ldarg.0
IL_019a: ldftn System.Void PersonalUpgradeUI::UpgradeAirBurst()
IL_01a0: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_01a5: ldarg.0
IL_01a6: ldfld PlayerState PersonalUpgradeUI::playerState
IL_01ab: ldc.i4.s 11
IL_01ad: callvirt System.Boolean PlayerState::CanGetUpgrade(PlayerState/Upgrade)
IL_01b2: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_01b7: stelem.ref
IL_01b8: dup
IL_01b9: ldc.i4.5
IL_01ba: ldstr "m.upgrade.name.personal.health1"
IL_01bf: ldarg.0
IL_01c0: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::health1
IL_01c5: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::icon
IL_01ca: ldarg.0
IL_01cb: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::health1
IL_01d0: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::img
IL_01d5: ldstr "m.upgrade.desc.personal.health1"
IL_01da: ldarg.0
IL_01db: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::health1
IL_01e0: ldfld System.Int32 PersonalUpgradeUI/UpgradePurchaseItem::cost
IL_01e5: ldloca.s V_6
IL_01e7: initobj System.Nullable`1<PediaDirector/Id>
IL_01ed: ldloc.s V_6
IL_01ef: ldarg.0
IL_01f0: ldftn System.Void PersonalUpgradeUI::UpgradeHealth1()
IL_01f6: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_01fb: ldarg.0
IL_01fc: ldfld PlayerState PersonalUpgradeUI::playerState
IL_0201: ldc.i4.0
IL_0202: callvirt System.Boolean PlayerState::CanGetUpgrade(PlayerState/Upgrade)
IL_0207: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_020c: stelem.ref
IL_020d: dup
IL_020e: ldc.i4.6
IL_020f: ldstr "m.upgrade.name.personal.health2"
IL_0214: ldarg.0
IL_0215: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::health2
IL_021a: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::icon
IL_021f: ldarg.0
IL_0220: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::health2
IL_0225: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::img
IL_022a: ldstr "m.upgrade.desc.personal.health2"
IL_022f: ldarg.0
IL_0230: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::health2
IL_0235: ldfld System.Int32 PersonalUpgradeUI/UpgradePurchaseItem::cost
IL_023a: ldloca.s V_7
IL_023c: initobj System.Nullable`1<PediaDirector/Id>
IL_0242: ldloc.s V_7
IL_0244: ldarg.0
IL_0245: ldftn System.Void PersonalUpgradeUI::UpgradeHealth2()
IL_024b: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_0250: ldarg.0
IL_0251: ldfld PlayerState PersonalUpgradeUI::playerState
IL_0256: ldc.i4.1
IL_0257: callvirt System.Boolean PlayerState::CanGetUpgrade(PlayerState/Upgrade)
IL_025c: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_0261: stelem.ref
IL_0262: dup
IL_0263: ldc.i4.7
IL_0264: ldstr "m.upgrade.name.personal.health3"
IL_0269: ldarg.0
IL_026a: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::health3
IL_026f: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::icon
IL_0274: ldarg.0
IL_0275: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::health3
IL_027a: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::img
IL_027f: ldstr "m.upgrade.desc.personal.health3"
IL_0284: ldarg.0
IL_0285: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::health3
IL_028a: ldfld System.Int32 PersonalUpgradeUI/UpgradePurchaseItem::cost
IL_028f: ldloca.s V_8
IL_0291: initobj System.Nullable`1<PediaDirector/Id>
IL_0297: ldloc.s V_8
IL_0299: ldarg.0
IL_029a: ldftn System.Void PersonalUpgradeUI::UpgradeHealth3()
IL_02a0: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_02a5: ldarg.0
IL_02a6: ldfld PlayerState PersonalUpgradeUI::playerState
IL_02ab: ldc.i4.2
IL_02ac: callvirt System.Boolean PlayerState::CanGetUpgrade(PlayerState/Upgrade)
IL_02b1: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_02b6: stelem.ref
IL_02b7: dup
IL_02b8: ldc.i4.8
IL_02b9: ldstr "m.upgrade.name.personal.energy1"
IL_02be: ldarg.0
IL_02bf: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::energy1
IL_02c4: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::icon
IL_02c9: ldarg.0
IL_02ca: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::energy1
IL_02cf: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::img
IL_02d4: ldstr "m.upgrade.desc.personal.energy1"
IL_02d9: ldarg.0
IL_02da: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::energy1
IL_02df: ldfld System.Int32 PersonalUpgradeUI/UpgradePurchaseItem::cost
IL_02e4: ldloca.s V_9
IL_02e6: initobj System.Nullable`1<PediaDirector/Id>
IL_02ec: ldloc.s V_9
IL_02ee: ldarg.0
IL_02ef: ldftn System.Void PersonalUpgradeUI::UpgradeEnergy1()
IL_02f5: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_02fa: ldarg.0
IL_02fb: ldfld PlayerState PersonalUpgradeUI::playerState
IL_0300: ldc.i4.3
IL_0301: callvirt System.Boolean PlayerState::CanGetUpgrade(PlayerState/Upgrade)
IL_0306: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_030b: stelem.ref
IL_030c: dup
IL_030d: ldc.i4.s 9
IL_030f: ldstr "m.upgrade.name.personal.energy2"
IL_0314: ldarg.0
IL_0315: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::energy2
IL_031a: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::icon
IL_031f: ldarg.0
IL_0320: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::energy2
IL_0325: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::img
IL_032a: ldstr "m.upgrade.desc.personal.energy2"
IL_032f: ldarg.0
IL_0330: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::energy2
IL_0335: ldfld System.Int32 PersonalUpgradeUI/UpgradePurchaseItem::cost
IL_033a: ldloca.s V_10
IL_033c: initobj System.Nullable`1<PediaDirector/Id>
IL_0342: ldloc.s V_10
IL_0344: ldarg.0
IL_0345: ldftn System.Void PersonalUpgradeUI::UpgradeEnergy2()
IL_034b: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_0350: ldarg.0
IL_0351: ldfld PlayerState PersonalUpgradeUI::playerState
IL_0356: ldc.i4.4
IL_0357: callvirt System.Boolean PlayerState::CanGetUpgrade(PlayerState/Upgrade)
IL_035c: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_0361: stelem.ref
IL_0362: dup
IL_0363: ldc.i4.s 10
IL_0365: ldstr "m.upgrade.name.personal.energy3"
IL_036a: ldarg.0
IL_036b: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::energy3
IL_0370: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::icon
IL_0375: ldarg.0
IL_0376: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::energy3
IL_037b: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::img
IL_0380: ldstr "m.upgrade.desc.personal.energy3"
IL_0385: ldarg.0
IL_0386: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::energy3
IL_038b: ldfld System.Int32 PersonalUpgradeUI/UpgradePurchaseItem::cost
IL_0390: ldloca.s V_11
IL_0392: initobj System.Nullable`1<PediaDirector/Id>
IL_0398: ldloc.s V_11
IL_039a: ldarg.0
IL_039b: ldftn System.Void PersonalUpgradeUI::UpgradeEnergy3()
IL_03a1: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_03a6: ldarg.0
IL_03a7: ldfld PlayerState PersonalUpgradeUI::playerState
IL_03ac: ldc.i4.5
IL_03ad: callvirt System.Boolean PlayerState::CanGetUpgrade(PlayerState/Upgrade)
IL_03b2: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_03b7: stelem.ref
IL_03b8: dup
IL_03b9: ldc.i4.s 11
IL_03bb: ldstr "m.upgrade.name.personal.ammo1"
IL_03c0: ldarg.0
IL_03c1: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::ammo1
IL_03c6: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::icon
IL_03cb: ldarg.0
IL_03cc: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::ammo1
IL_03d1: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::img
IL_03d6: ldstr "m.upgrade.desc.personal.ammo1"
IL_03db: ldarg.0
IL_03dc: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::ammo1
IL_03e1: ldfld System.Int32 PersonalUpgradeUI/UpgradePurchaseItem::cost
IL_03e6: ldloca.s V_12
IL_03e8: initobj System.Nullable`1<PediaDirector/Id>
IL_03ee: ldloc.s V_12
IL_03f0: ldarg.0
IL_03f1: ldftn System.Void PersonalUpgradeUI::UpgradeAmmo1()
IL_03f7: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_03fc: ldarg.0
IL_03fd: ldfld PlayerState PersonalUpgradeUI::playerState
IL_0402: ldc.i4.6
IL_0403: callvirt System.Boolean PlayerState::CanGetUpgrade(PlayerState/Upgrade)
IL_0408: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_040d: stelem.ref
IL_040e: dup
IL_040f: ldc.i4.s 12
IL_0411: ldstr "m.upgrade.name.personal.ammo2"
IL_0416: ldarg.0
IL_0417: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::ammo2
IL_041c: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::icon
IL_0421: ldarg.0
IL_0422: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::ammo2
IL_0427: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::img
IL_042c: ldstr "m.upgrade.desc.personal.ammo2"
IL_0431: ldarg.0
IL_0432: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::ammo2
IL_0437: ldfld System.Int32 PersonalUpgradeUI/UpgradePurchaseItem::cost
IL_043c: ldloca.s V_13
IL_043e: initobj System.Nullable`1<PediaDirector/Id>
IL_0444: ldloc.s V_13
IL_0446: ldarg.0
IL_0447: ldftn System.Void PersonalUpgradeUI::UpgradeAmmo2()
IL_044d: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_0452: ldarg.0
IL_0453: ldfld PlayerState PersonalUpgradeUI::playerState
IL_0458: ldc.i4.7
IL_0459: callvirt System.Boolean PlayerState::CanGetUpgrade(PlayerState/Upgrade)
IL_045e: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_0463: stelem.ref
IL_0464: dup
IL_0465: ldc.i4.s 13
IL_0467: ldstr "m.upgrade.name.personal.ammo3"
IL_046c: ldarg.0
IL_046d: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::ammo3
IL_0472: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::icon
IL_0477: ldarg.0
IL_0478: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::ammo3
IL_047d: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::img
IL_0482: ldstr "m.upgrade.desc.personal.ammo3"
IL_0487: ldarg.0
IL_0488: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::ammo3
IL_048d: ldfld System.Int32 PersonalUpgradeUI/UpgradePurchaseItem::cost
IL_0492: ldloca.s V_14
IL_0494: initobj System.Nullable`1<PediaDirector/Id>
IL_049a: ldloc.s V_14
IL_049c: ldarg.0
IL_049d: ldftn System.Void PersonalUpgradeUI::UpgradeAmmo3()
IL_04a3: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_04a8: ldarg.0
IL_04a9: ldfld PlayerState PersonalUpgradeUI::playerState
IL_04ae: ldc.i4.8
IL_04af: callvirt System.Boolean PlayerState::CanGetUpgrade(PlayerState/Upgrade)
IL_04b4: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_04b9: stelem.ref
IL_04ba: stloc.0
IL_04bb: call T SRSingleton`1<GameContext>::get_Instance()
IL_04c0: callvirt UITemplates GameContext::get_UITemplates()
IL_04c5: ldarg.0
IL_04c6: ldfld UnityEngine.Sprite PersonalUpgradeUI::titleIcon
IL_04cb: ldstr "ui"
IL_04d0: ldstr "t.personal_upgrades"
IL_04d5: call System.String MessageUtil::Qualify(System.String,System.String)
IL_04da: ldloc.0
IL_04db: ldarg.0
IL_04dc: dup
IL_04dd: ldvirtftn System.Void BaseUI::Close()
IL_04e3: newobj System.Void PurchaseUI/OnClose::.ctor(System.Object,System.IntPtr)
IL_04e8: callvirt UnityEngine.GameObject UITemplates::CreatePurchaseUI(UnityEngine.Sprite,System.String,PurchaseUI/Purchasable[],PurchaseUI/OnClose)
IL_04ed: box UnityEngine.GameObject
IL_04f2: stloc V_15
IL_04f6: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::Spawn_Player_Upgrades_UI
IL_04fb: ldarg 
IL_04ff: ldloca V_15
IL_0503: ldnull
IL_0504: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_0509: stloc V_16
IL_050d: ldloc V_15
IL_0511: unbox.any UnityEngine.GameObject
IL_0516: ret

*/

// PersonalUpgradeUI
protected GameObject CreatePurchaseUI()
{
	object obj = null;
	PurchaseUI.Purchasable[] purchasables = new PurchaseUI.Purchasable[]
	{
		new PurchaseUI.Purchasable("m.upgrade.name.personal.liquid_slot", this.liquidSlot.icon, this.liquidSlot.img, "m.upgrade.desc.personal.liquid_slot", this.liquidSlot.cost, null, new UnityAction(this.UpgradeLiquidSlot), this.playerState.CanGetUpgrade(PlayerState.Upgrade.LIQUID_SLOT)),
		new PurchaseUI.Purchasable("m.upgrade.name.personal.jetpack", this.jetpack.icon, this.jetpack.img, "m.upgrade.desc.personal.jetpack", this.jetpack.cost, null, new UnityAction(this.UpgradeJetpack), this.playerState.CanGetUpgrade(PlayerState.Upgrade.JETPACK)),
		new PurchaseUI.Purchasable("m.upgrade.name.personal.jetpack_efficiency", this.jetpackEfficiency.icon, this.jetpackEfficiency.img, "m.upgrade.desc.personal.jetpack_efficiency", this.jetpackEfficiency.cost, null, new UnityAction(this.UpgradeJetpackEfficiency), this.playerState.CanGetUpgrade(PlayerState.Upgrade.JETPACK_EFFICIENCY)),
		new PurchaseUI.Purchasable("m.upgrade.name.personal.run_efficiency", this.runEfficiency.icon, this.runEfficiency.img, "m.upgrade.desc.personal.run_efficiency", this.runEfficiency.cost, null, new UnityAction(this.UpgradeRunEfficiency), this.playerState.CanGetUpgrade(PlayerState.Upgrade.RUN_EFFICIENCY)),
		new PurchaseUI.Purchasable("m.upgrade.name.personal.air_burst", this.airBurst.icon, this.airBurst.img, "m.upgrade.desc.personal.air_burst", this.airBurst.cost, null, new UnityAction(this.UpgradeAirBurst), this.playerState.CanGetUpgrade(PlayerState.Upgrade.AIR_BURST)),
		new PurchaseUI.Purchasable("m.upgrade.name.personal.health1", this.health1.icon, this.health1.img, "m.upgrade.desc.personal.health1", this.health1.cost, null, new UnityAction(this.UpgradeHealth1), this.playerState.CanGetUpgrade(PlayerState.Upgrade.HEALTH_1)),
		new PurchaseUI.Purchasable("m.upgrade.name.personal.health2", this.health2.icon, this.health2.img, "m.upgrade.desc.personal.health2", this.health2.cost, null, new UnityAction(this.UpgradeHealth2), this.playerState.CanGetUpgrade(PlayerState.Upgrade.HEALTH_2)),
		new PurchaseUI.Purchasable("m.upgrade.name.personal.health3", this.health3.icon, this.health3.img, "m.upgrade.desc.personal.health3", this.health3.cost, null, new UnityAction(this.UpgradeHealth3), this.playerState.CanGetUpgrade(PlayerState.Upgrade.HEALTH_3)),
		new PurchaseUI.Purchasable("m.upgrade.name.personal.energy1", this.energy1.icon, this.energy1.img, "m.upgrade.desc.personal.energy1", this.energy1.cost, null, new UnityAction(this.UpgradeEnergy1), this.playerState.CanGetUpgrade(PlayerState.Upgrade.ENERGY_1)),
		new PurchaseUI.Purchasable("m.upgrade.name.personal.energy2", this.energy2.icon, this.energy2.img, "m.upgrade.desc.personal.energy2", this.energy2.cost, null, new UnityAction(this.UpgradeEnergy2), this.playerState.CanGetUpgrade(PlayerState.Upgrade.ENERGY_2)),
		new PurchaseUI.Purchasable("m.upgrade.name.personal.energy3", this.energy3.icon, this.energy3.img, "m.upgrade.desc.personal.energy3", this.energy3.cost, null, new UnityAction(this.UpgradeEnergy3), this.playerState.CanGetUpgrade(PlayerState.Upgrade.ENERGY_3)),
		new PurchaseUI.Purchasable("m.upgrade.name.personal.ammo1", this.ammo1.icon, this.ammo1.img, "m.upgrade.desc.personal.ammo1", this.ammo1.cost, null, new UnityAction(this.UpgradeAmmo1), this.playerState.CanGetUpgrade(PlayerState.Upgrade.AMMO_1)),
		new PurchaseUI.Purchasable("m.upgrade.name.personal.ammo2", this.ammo2.icon, this.ammo2.img, "m.upgrade.desc.personal.ammo2", this.ammo2.cost, null, new UnityAction(this.UpgradeAmmo2), this.playerState.CanGetUpgrade(PlayerState.Upgrade.AMMO_2)),
		new PurchaseUI.Purchasable("m.upgrade.name.personal.ammo3", this.ammo3.icon, this.ammo3.img, "m.upgrade.desc.personal.ammo3", this.ammo3.cost, null, new UnityAction(this.UpgradeAmmo3), this.playerState.CanGetUpgrade(PlayerState.Upgrade.AMMO_3))
	};
	obj = SRSingleton<GameContext>.Instance.UITemplates.CreatePurchaseUI(this.titleIcon, MessageUtil.Qualify("ui", "t.personal_upgrades"), purchasables, new PurchaseUI.OnClose(this.Close));
	_hook_result hook_result = SiscosHooks.call(HOOK_ID.Spawn_Player_Upgrades_UI, this, ref obj, null);
	return (GameObject)obj;
}

/*
IL_0000: ldnull
IL_0001: stloc V_4
IL_0005: nop
IL_0006: ldarg.0
IL_0007: ldfld System.Boolean PlayerDeathHandler::deathInProgress
IL_000c: brfalse IL_0012
IL_0011: ret
IL_0012: nop
IL_0013: ldarg.0
IL_0014: ldc.i4.1
IL_0015: stfld System.Boolean PlayerDeathHandler::deathInProgress
IL_001a: call T SRSingleton`1<SceneContext>::get_Instance()
IL_001f: callvirt TimeDirector SceneContext::get_TimeDirector()
IL_0024: stloc.0
IL_0025: call T SRSingleton`1<SceneContext>::get_Instance()
IL_002a: callvirt PlayerState SceneContext::get_PlayerState()
IL_002f: stloc.1
IL_0030: ldloc.1
IL_0031: callvirt GameModeSettings PlayerState::get_ModeSettings()
IL_0036: ldfld System.Boolean GameModeSettings::hoursTilDawnOnDeath
IL_003b: brfalse IL_004b
IL_0040: ldloc.0
IL_0041: callvirt System.Single TimeDirector::GetNextDawnAfterNextDusk()
IL_0046: br IL_005d
IL_004b: nop
IL_004c: ldloc.0
IL_004d: ldloc.1
IL_004e: callvirt GameModeSettings PlayerState::get_ModeSettings()
IL_0053: ldfld System.Single GameModeSettings::hoursLostOnDeath
IL_0058: callvirt System.Single TimeDirector::HoursFromNow(System.Single)
IL_005d: stloc.2
IL_005e: ldloc.1
IL_005f: callvirt GameModeSettings PlayerState::get_ModeSettings()
IL_0064: ldfld System.Boolean GameModeSettings::hoursTilDawnOnDeath
IL_0069: brfalse IL_00a5
IL_006e: call T SRSingleton`1<SceneContext>::get_Instance()
IL_0073: callvirt TimeDirector SceneContext::get_TimeDirector()
IL_0078: callvirt System.Single TimeDirector::CurrHour()
IL_007d: stloc.3
IL_007e: ldloc.3
IL_007f: ldc.r4 10
IL_0084: bge.un IL_00a6
IL_0089: ldloc.3
IL_008a: ldc.r4 6
IL_008f: blt.un IL_00a7
IL_0094: call T SRSingleton`1<SceneContext>::get_Instance()
IL_0099: callvirt AchievementsDirector SceneContext::get_AchievementsDirector()
IL_009e: ldc.i4.4
IL_009f: ldc.i4.1
IL_00a0: callvirt System.Void AchievementsDirector::AddToStat(AchievementsDirector/IntStat,System.Int32)
IL_00a5: nop
IL_00a6: nop
IL_00a7: nop
IL_00a8: call T SRSingleton`1<SceneContext>::get_Instance()
IL_00ad: callvirt AchievementsDirector SceneContext::get_AchievementsDirector()
IL_00b2: ldc.i4.0
IL_00b3: ldc.i4.1
IL_00b4: callvirt System.Void AchievementsDirector::AddToStat(AchievementsDirector/GameIntStat,System.Int32)
IL_00b9: ldloc.1
IL_00ba: callvirt GameModeSettings PlayerState::get_ModeSettings()
IL_00bf: ldfld System.Single GameModeSettings::pctCurrencyLostOnDeath
IL_00c4: ldc.r4 0
IL_00c9: ble.un IL_00ed
IL_00ce: ldloc.1
IL_00cf: ldloc.1
IL_00d0: callvirt System.Int32 PlayerState::GetCurrency()
IL_00d5: conv.r4
IL_00d6: ldloc.1
IL_00d7: callvirt GameModeSettings PlayerState::get_ModeSettings()
IL_00dc: ldfld System.Single GameModeSettings::pctCurrencyLostOnDeath
IL_00e1: mul
IL_00e2: call System.Int32 UnityEngine.Mathf::FloorToInt(System.Single)
IL_00e7: ldc.i4.1
IL_00e8: callvirt System.Void PlayerState::SpendCurrency(System.Int32,System.Boolean)
IL_00ed: nop
IL_00ee: ldarg.0
IL_00ef: call !!0 UnityEngine.Component::GetComponent<LockOnDeath>()
IL_00f4: ldloc.2
IL_00f5: ldc.r4 5
IL_00fa: ldarg.0
IL_00fb: ldftn System.Void PlayerDeathHandler::<OnDeath>m__22()
IL_0101: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_0106: callvirt System.Void LockOnDeath::LockUntil(System.Single,System.Single,UnityEngine.Events.UnityAction)
IL_010b: ldstr "PlayerDeath"
IL_0110: ldnull
IL_0111: call UnityEngine.Analytics.AnalyticsResult UnityEngine.Analytics.Analytics::CustomEvent(System.String,System.Collections.Generic.IDictionary`2<System.String,System.Object>)
IL_0116: pop
IL_0117: ldarg.0
IL_0118: ldarg.0
IL_0119: ldc.i4.1
IL_011a: ldc.r4 1
IL_011f: ldnull
IL_0120: call System.Collections.IEnumerator PlayerDeathHandler::ResetPlayer(System.Boolean,System.Single,UnityEngine.Events.UnityAction)
IL_0125: call UnityEngine.Coroutine UnityEngine.MonoBehaviour::StartCoroutine(System.Collections.IEnumerator)
IL_012a: pop
IL_012b: ldarg.0
IL_012c: ldarg.0
IL_012d: call System.Collections.IEnumerator PlayerDeathHandler::DisplayDeathUI()
IL_0132: call UnityEngine.Coroutine UnityEngine.MonoBehaviour::StartCoroutine(System.Collections.IEnumerator)
IL_0137: pop
IL_0138: nop
IL_0139: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::Player_Death
IL_013e: ldarg 
IL_0142: ldloca V_4
IL_0146: ldnull
IL_0147: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_014c: pop
IL_014d: ret

*/

// PlayerDeathHandler
public void OnDeath()
{
	object obj = null;
	if (this.deathInProgress)
	{
		return;
	}
	this.deathInProgress = true;
	TimeDirector timeDirector = SRSingleton<SceneContext>.Instance.TimeDirector;
	PlayerState playerState = SRSingleton<SceneContext>.Instance.PlayerState;
	float unlockWorldTime = (!playerState.ModeSettings.hoursTilDawnOnDeath) ? timeDirector.HoursFromNow(playerState.ModeSettings.hoursLostOnDeath) : timeDirector.GetNextDawnAfterNextDusk();
	if (playerState.ModeSettings.hoursTilDawnOnDeath)
	{
		float num = SRSingleton<SceneContext>.Instance.TimeDirector.CurrHour();
		if (num < 10f)
		{
			if (num >= 6f)
			{
				SRSingleton<SceneContext>.Instance.AchievementsDirector.AddToStat(AchievementsDirector.IntStat.DEATH_BEFORE_10AM, 1);
			}
		}
	}
	SRSingleton<SceneContext>.Instance.AchievementsDirector.AddToStat(AchievementsDirector.GameIntStat.DEATHS, 1);
	if (playerState.ModeSettings.pctCurrencyLostOnDeath > 0f)
	{
		playerState.SpendCurrency(Mathf.FloorToInt((float)playerState.GetCurrency() * playerState.ModeSettings.pctCurrencyLostOnDeath), true);
	}
	base.GetComponent<LockOnDeath>().LockUntil(unlockWorldTime, 5f, delegate
	{
		this.deathInProgress = false;
	});
	Analytics.CustomEvent("PlayerDeath", null);
	base.StartCoroutine(this.ResetPlayer(true, 1f, null));
	base.StartCoroutine(this.DisplayDeathUI());
	SiscosHooks.call(HOOK_ID.Player_Death, this, ref obj, null);
}

/*
IL_0000: ldnull
IL_0001: stloc V_3
IL_0005: nop
IL_0006: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::Player_MoneyChanged
IL_000b: ldarg 
IL_000f: ldloca V_3
IL_0013: ldc.i4 1
IL_0018: newarr System.Object
IL_001d: dup
IL_001e: ldc.i4 0
IL_0023: ldarg adjust
IL_0027: box System.Int32
IL_002c: stelem.ref
IL_002d: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_0032: stloc V_4
IL_0036: ldloc V_4
IL_003a: ldfld System.Boolean SR_PluginLoader._hook_result::abort
IL_003f: brfalse IL_0045
IL_0044: ret
IL_0045: nop
IL_0046: ldloc V_4
IL_004a: ldfld System.Object[] SR_PluginLoader._hook_result::args
IL_004f: brfalse IL_006a
IL_0054: ldloc V_4
IL_0058: ldfld System.Object[] SR_PluginLoader._hook_result::args
IL_005d: ldc.i4 0
IL_0062: ldelem.ref
IL_0063: unbox.any System.Int32
IL_0068: starg.s adjust
IL_006a: nop
IL_006b: ldarg.0
IL_006c: dup
IL_006d: ldfld System.Int32 PlayerState::currency
IL_0072: ldarg.1
IL_0073: add
IL_0074: stfld System.Int32 PlayerState::currency
IL_0079: ldarg.0
IL_007a: dup
IL_007b: ldfld System.Int32 PlayerState::currencyEverCollected
IL_0080: ldarg.1
IL_0081: add
IL_0082: stfld System.Int32 PlayerState::currencyEverCollected
IL_0087: ldarg.0
IL_0088: ldfld System.Int32 PlayerState::currencyEverCollected
IL_008d: ldarg.0
IL_008e: ldfld System.Int32 PlayerState::currencyPerProgress
IL_0093: div
IL_0094: ldarg.0
IL_0095: ldfld System.Int32 PlayerState::currencyEverCollected
IL_009a: ldarg.1
IL_009b: sub
IL_009c: ldarg.0
IL_009d: ldfld System.Int32 PlayerState::currencyPerProgress
IL_00a2: div
IL_00a3: sub
IL_00a4: stloc.0
IL_00a5: ldloc.0
IL_00a6: ldc.i4.0
IL_00a7: ble IL_00d6
IL_00ac: call T SRSingleton`1<SceneContext>::get_Instance()
IL_00b1: callvirt ProgressDirector SceneContext::get_ProgressDirector()
IL_00b6: stloc.1
IL_00b7: ldc.i4.0
IL_00b8: stloc.2
IL_00b9: br IL_00cf
IL_00be: nop
IL_00bf: ldloc.1
IL_00c0: ldarg.0
IL_00c1: ldfld ProgressDirector/ProgressType PlayerState::currencyProgressType
IL_00c6: callvirt System.Void ProgressDirector::AddProgress(ProgressDirector/ProgressType)
IL_00cb: ldloc.2
IL_00cc: ldc.i4.1
IL_00cd: add
IL_00ce: stloc.2
IL_00cf: ldloc.2
IL_00d0: ldloc.0
IL_00d1: blt IL_00be
IL_00d6: nop
IL_00d7: ldarg.1
IL_00d8: ldc.i4.0
IL_00d9: ble IL_00f8
IL_00de: ldarg.0
IL_00df: ldfld AchievementsDirector PlayerState::achieveDir
IL_00e4: ldc.i4.2
IL_00e5: ldarg.1
IL_00e6: callvirt System.Void AchievementsDirector::AddToStat(AchievementsDirector/IntStat,System.Int32)
IL_00eb: ldarg.0
IL_00ec: ldfld AchievementsDirector PlayerState::achieveDir
IL_00f1: ldc.i4.3
IL_00f2: ldarg.1
IL_00f3: callvirt System.Void AchievementsDirector::AddToStat(AchievementsDirector/IntStat,System.Int32)
IL_00f8: ret

*/

// PlayerState
public void AddCurrency(int adjust)
{
	object obj = null;
	_hook_result hook_result = SiscosHooks.call(HOOK_ID.Player_MoneyChanged, this, ref obj, new object[]
	{
		adjust
	});
	if (hook_result.abort)
	{
		return;
	}
	if (hook_result.args != null)
	{
		adjust = (int)hook_result.args[0];
	}
	this.currency += adjust;
	this.currencyEverCollected += adjust;
	int num = this.currencyEverCollected / this.currencyPerProgress - (this.currencyEverCollected - adjust) / this.currencyPerProgress;
	if (num > 0)
	{
		ProgressDirector progressDirector = SRSingleton<SceneContext>.Instance.ProgressDirector;
		for (int i = 0; i < num; i++)
		{
			progressDirector.AddProgress(this.currencyProgressType);
		}
	}
	if (adjust > 0)
	{
		this.achieveDir.AddToStat(AchievementsDirector.IntStat.DAY_CURRENCY, adjust);
		this.achieveDir.AddToStat(AchievementsDirector.IntStat.CURRENCY, adjust);
	}
}

/*
IL_0000: ldc.i4.0
IL_0001: box System.Int32
IL_0006: stloc V_2
IL_000a: nop
IL_000b: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::Player_AddRads
IL_0010: ldarg 
IL_0014: ldloca V_2
IL_0018: ldc.i4 1
IL_001d: newarr System.Object
IL_0022: dup
IL_0023: ldc.i4 0
IL_0028: ldarg rads
IL_002c: box System.Single
IL_0031: stelem.ref
IL_0032: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_0037: stloc V_3
IL_003b: ldloc V_3
IL_003f: ldfld System.Boolean SR_PluginLoader._hook_result::abort
IL_0044: brfalse IL_0053
IL_0049: ldloc V_2
IL_004d: unbox.any System.Int32
IL_0052: ret
IL_0053: nop
IL_0054: ldloc V_3
IL_0058: ldfld System.Object[] SR_PluginLoader._hook_result::args
IL_005d: brfalse IL_0078
IL_0062: ldloc V_3
IL_0066: ldfld System.Object[] SR_PluginLoader._hook_result::args
IL_006b: ldc.i4 0
IL_0070: ldelem.ref
IL_0071: unbox.any System.Single
IL_0076: starg.s rads
IL_0078: nop
IL_0079: ldarg.0
IL_007a: dup
IL_007b: ldfld System.Single PlayerState::currRads
IL_0080: ldarg.1
IL_0081: add
IL_0082: stfld System.Single PlayerState::currRads
IL_0087: ldarg.0
IL_0088: ldarg.0
IL_0089: ldfld TimeDirector PlayerState::timeDir
IL_008e: callvirt System.Single TimeDirector::WorldTime()
IL_0093: ldc.r4 60
IL_0098: ldarg.0
IL_0099: ldfld System.Single PlayerState::currRads
IL_009e: ldarg.0
IL_009f: ldfld System.Int32 PlayerState::maxRads
IL_00a4: conv.r4
IL_00a5: blt.un IL_00b5
IL_00aa: ldarg.0
IL_00ab: ldfld System.Single PlayerState::fullRadRecoveryDelay
IL_00b0: br IL_00bc
IL_00b5: nop
IL_00b6: ldarg.0
IL_00b7: ldfld System.Single PlayerState::nonfullRadRecoveryDelay
IL_00bc: mul
IL_00bd: add
IL_00be: stfld System.Single PlayerState::radRecoverAfter
IL_00c3: ldarg.0
IL_00c4: ldfld System.Single PlayerState::currRads
IL_00c9: ldarg.0
IL_00ca: ldfld System.Int32 PlayerState::maxRads
IL_00cf: conv.r4
IL_00d0: ble.un IL_0112
IL_00d5: ldarg.0
IL_00d6: ldfld System.Single PlayerState::currRads
IL_00db: ldarg.0
IL_00dc: ldfld System.Int32 PlayerState::maxRads
IL_00e1: conv.r4
IL_00e2: sub
IL_00e3: ldarg.0
IL_00e4: ldfld System.Int32 PlayerState::radUnitDamage
IL_00e9: conv.r4
IL_00ea: div
IL_00eb: call System.Int32 UnityEngine.Mathf::FloorToInt(System.Single)
IL_00f0: stloc.0
IL_00f1: ldloc.0
IL_00f2: ldc.i4.0
IL_00f3: ble IL_0113
IL_00f8: ldarg.0
IL_00f9: ldfld System.Int32 PlayerState::radUnitDamage
IL_00fe: ldloc.0
IL_00ff: mul
IL_0100: stloc.1
IL_0101: ldarg.0
IL_0102: dup
IL_0103: ldfld System.Single PlayerState::currRads
IL_0108: ldloc.1
IL_0109: conv.r4
IL_010a: sub
IL_010b: stfld System.Single PlayerState::currRads
IL_0110: ldloc.1
IL_0111: ret
IL_0112: nop
IL_0113: nop
IL_0114: ldc.i4.0
IL_0115: ret

*/

// PlayerState
public int AddRads(float rads)
{
	object obj = 0;
	_hook_result hook_result = SiscosHooks.call(HOOK_ID.Player_AddRads, this, ref obj, new object[]
	{
		rads
	});
	if (hook_result.abort)
	{
		return (int)obj;
	}
	if (hook_result.args != null)
	{
		rads = (float)hook_result.args[0];
	}
	this.currRads += rads;
	this.radRecoverAfter = this.timeDir.WorldTime() + 60f * ((this.currRads < (float)this.maxRads) ? this.nonfullRadRecoveryDelay : this.fullRadRecoveryDelay);
	if (this.currRads > (float)this.maxRads)
	{
		int num = Mathf.FloorToInt((this.currRads - (float)this.maxRads) / (float)this.radUnitDamage);
		if (num > 0)
		{
			int num2 = this.radUnitDamage * num;
			this.currRads -= (float)num2;
			return num2;
		}
	}
	return 0;
}

/*
IL_0000: ldnull
IL_0001: stloc V_4
IL_0005: nop
IL_0006: call T SRSingleton`1<SceneContext>::get_Instance()
IL_000b: callvirt UnityEngine.GameObject SceneContext::get_Player()
IL_0010: callvirt !!0 UnityEngine.GameObject::GetComponent<EnergyJetpack>()
IL_0015: stloc.0
IL_0016: call T SRSingleton`1<SceneContext>::get_Instance()
IL_001b: callvirt UnityEngine.GameObject SceneContext::get_Player()
IL_0020: callvirt !!0 UnityEngine.GameObject::GetComponent<StaminaRun>()
IL_0025: stloc.1
IL_0026: call T SRSingleton`1<SceneContext>::get_Instance()
IL_002b: callvirt UnityEngine.GameObject SceneContext::get_Player()
IL_0030: callvirt !!0 UnityEngine.GameObject::GetComponentInChildren<WeaponVacuum>()
IL_0035: stloc.2
IL_0036: ldarg.1
IL_0037: stloc.3
IL_0038: ldloc.3
IL_0039: switch IL_00d6,IL_014d,IL_01c4,IL_022e,IL_029e,IL_030e,IL_0371,IL_03a5,IL_03d9,IL_007b,IL_0094,IL_00ca,IL_00af,IL_0401
IL_0076: br IL_0412
IL_007b: ldloc.0
IL_007c: ldc.i4.1
IL_007d: callvirt System.Void EnergyJetpack::set_HasJetpack(System.Boolean)
IL_0082: ldarg.0
IL_0083: ldc.i4.s 10
IL_0085: ldc.r4 120
IL_008a: call System.Void PlayerState::MaybeAddUpgradeLock(PlayerState/Upgrade,System.Single)
IL_008f: br IL_0412
IL_0094: ldloc.0
IL_0095: ldloc.0
IL_0096: callvirt System.Single EnergyJetpack::get_Efficiency()
IL_009b: ldc.r4 0.8
IL_00a0: call System.Single System.Math::Min(System.Single,System.Single)
IL_00a5: callvirt System.Void EnergyJetpack::set_Efficiency(System.Single)
IL_00aa: br IL_0412
IL_00af: ldloc.1
IL_00b0: ldloc.1
IL_00b1: callvirt System.Single StaminaRun::get_Efficiency()
IL_00b6: ldc.r4 0.8333
IL_00bb: call System.Single System.Math::Min(System.Single,System.Single)
IL_00c0: callvirt System.Void StaminaRun::set_Efficiency(System.Single)
IL_00c5: br IL_0412
IL_00ca: ldloc.2
IL_00cb: ldc.i4.1
IL_00cc: callvirt System.Void WeaponVacuum::set_HasAirBurst(System.Boolean)
IL_00d1: br IL_0412
IL_00d6: ldarg.0
IL_00d7: ldarg.0
IL_00d8: ldfld System.Int32 PlayerState::maxHealth
IL_00dd: ldarg.0
IL_00de: ldfld System.Int32 PlayerState::defaultMaxHealth
IL_00e3: conv.r4
IL_00e4: ldc.r4 1.5
IL_00e9: mul
IL_00ea: call System.Int32 UnityEngine.Mathf::RoundToInt(System.Single)
IL_00ef: call System.Int32 System.Math::Max(System.Int32,System.Int32)
IL_00f4: stfld System.Int32 PlayerState::maxHealth
IL_00f9: ldarg.0
IL_00fa: ldfld System.Single PlayerState::currHealth
IL_00ff: ldarg.0
IL_0100: ldfld System.Int32 PlayerState::maxHealth
IL_0105: conv.r4
IL_0106: bge.un IL_013b
IL_010b: ldarg.0
IL_010c: ldarg.0
IL_010d: ldfld System.Single PlayerState::healthBurstAfter
IL_0112: ldarg.0
IL_0113: ldfld TimeDirector PlayerState::timeDir
IL_0118: callvirt System.Single TimeDirector::WorldTime()
IL_011d: ldc.r4 60
IL_0122: ldarg.0
IL_0123: ldfld System.Single PlayerState::healthBurstAmount
IL_0128: mul
IL_0129: ldarg.0
IL_012a: ldfld System.Single PlayerState::healthRecoveredPerSecond
IL_012f: div
IL_0130: add
IL_0131: call System.Single System.Math::Min(System.Single,System.Single)
IL_0136: stfld System.Single PlayerState::healthBurstAfter
IL_013b: nop
IL_013c: ldarg.0
IL_013d: ldc.i4.1
IL_013e: ldc.r4 48
IL_0143: call System.Void PlayerState::MaybeAddUpgradeLock(PlayerState/Upgrade,System.Single)
IL_0148: br IL_0412
IL_014d: ldarg.0
IL_014e: ldarg.0
IL_014f: ldfld System.Int32 PlayerState::maxHealth
IL_0154: ldarg.0
IL_0155: ldfld System.Int32 PlayerState::defaultMaxHealth
IL_015a: conv.r4
IL_015b: ldc.r4 2
IL_0160: mul
IL_0161: call System.Int32 UnityEngine.Mathf::RoundToInt(System.Single)
IL_0166: call System.Int32 System.Math::Max(System.Int32,System.Int32)
IL_016b: stfld System.Int32 PlayerState::maxHealth
IL_0170: ldarg.0
IL_0171: ldfld System.Single PlayerState::currHealth
IL_0176: ldarg.0
IL_0177: ldfld System.Int32 PlayerState::maxHealth
IL_017c: conv.r4
IL_017d: bge.un IL_01b2
IL_0182: ldarg.0
IL_0183: ldarg.0
IL_0184: ldfld System.Single PlayerState::healthBurstAfter
IL_0189: ldarg.0
IL_018a: ldfld TimeDirector PlayerState::timeDir
IL_018f: callvirt System.Single TimeDirector::WorldTime()
IL_0194: ldc.r4 60
IL_0199: ldarg.0
IL_019a: ldfld System.Single PlayerState::healthBurstAmount
IL_019f: mul
IL_01a0: ldarg.0
IL_01a1: ldfld System.Single PlayerState::healthRecoveredPerSecond
IL_01a6: div
IL_01a7: add
IL_01a8: call System.Single System.Math::Min(System.Single,System.Single)
IL_01ad: stfld System.Single PlayerState::healthBurstAfter
IL_01b2: nop
IL_01b3: ldarg.0
IL_01b4: ldc.i4.2
IL_01b5: ldc.r4 72
IL_01ba: call System.Void PlayerState::MaybeAddUpgradeLock(PlayerState/Upgrade,System.Single)
IL_01bf: br IL_0412
IL_01c4: ldarg.0
IL_01c5: ldarg.0
IL_01c6: ldfld System.Int32 PlayerState::maxHealth
IL_01cb: ldarg.0
IL_01cc: ldfld System.Int32 PlayerState::defaultMaxHealth
IL_01d1: conv.r4
IL_01d2: ldc.r4 2.5
IL_01d7: mul
IL_01d8: call System.Int32 UnityEngine.Mathf::RoundToInt(System.Single)
IL_01dd: call System.Int32 System.Math::Max(System.Int32,System.Int32)
IL_01e2: stfld System.Int32 PlayerState::maxHealth
IL_01e7: ldarg.0
IL_01e8: ldfld System.Single PlayerState::currHealth
IL_01ed: ldarg.0
IL_01ee: ldfld System.Int32 PlayerState::maxHealth
IL_01f3: conv.r4
IL_01f4: bge.un IL_0229
IL_01f9: ldarg.0
IL_01fa: ldarg.0
IL_01fb: ldfld System.Single PlayerState::healthBurstAfter
IL_0200: ldarg.0
IL_0201: ldfld TimeDirector PlayerState::timeDir
IL_0206: callvirt System.Single TimeDirector::WorldTime()
IL_020b: ldc.r4 60
IL_0210: ldarg.0
IL_0211: ldfld System.Single PlayerState::healthBurstAmount
IL_0216: mul
IL_0217: ldarg.0
IL_0218: ldfld System.Single PlayerState::healthRecoveredPerSecond
IL_021d: div
IL_021e: add
IL_021f: call System.Single System.Math::Min(System.Single,System.Single)
IL_0224: stfld System.Single PlayerState::healthBurstAfter
IL_0229: br IL_0412
IL_022e: ldarg.0
IL_022f: ldarg.0
IL_0230: ldfld System.Int32 PlayerState::maxEnergy
IL_0235: ldarg.0
IL_0236: ldfld System.Int32 PlayerState::defaultMaxEnergy
IL_023b: conv.r4
IL_023c: ldc.r4 1.5
IL_0241: mul
IL_0242: call System.Int32 UnityEngine.Mathf::RoundToInt(System.Single)
IL_0247: call System.Int32 System.Math::Max(System.Int32,System.Int32)
IL_024c: stfld System.Int32 PlayerState::maxEnergy
IL_0251: ldarg.0
IL_0252: ldfld System.Single PlayerState::currEnergy
IL_0257: ldarg.0
IL_0258: ldfld System.Int32 PlayerState::maxEnergy
IL_025d: conv.r4
IL_025e: bge.un IL_028c
IL_0263: ldarg.0
IL_0264: ldarg.0
IL_0265: ldfld System.Single PlayerState::energyRecoverAfter
IL_026a: ldarg.0
IL_026b: ldfld TimeDirector PlayerState::timeDir
IL_0270: callvirt System.Single TimeDirector::WorldTime()
IL_0275: ldc.r4 60
IL_027a: ldarg.0
IL_027b: ldfld System.Single PlayerState::energyRecoveryDelay
IL_0280: mul
IL_0281: add
IL_0282: call System.Single System.Math::Min(System.Single,System.Single)
IL_0287: stfld System.Single PlayerState::energyRecoverAfter
IL_028c: nop
IL_028d: ldarg.0
IL_028e: ldc.i4.4
IL_028f: ldc.r4 48
IL_0294: call System.Void PlayerState::MaybeAddUpgradeLock(PlayerState/Upgrade,System.Single)
IL_0299: br IL_0412
IL_029e: ldarg.0
IL_029f: ldarg.0
IL_02a0: ldfld System.Int32 PlayerState::maxEnergy
IL_02a5: ldarg.0
IL_02a6: ldfld System.Int32 PlayerState::defaultMaxEnergy
IL_02ab: conv.r4
IL_02ac: ldc.r4 2
IL_02b1: mul
IL_02b2: call System.Int32 UnityEngine.Mathf::RoundToInt(System.Single)
IL_02b7: call System.Int32 System.Math::Max(System.Int32,System.Int32)
IL_02bc: stfld System.Int32 PlayerState::maxEnergy
IL_02c1: ldarg.0
IL_02c2: ldfld System.Single PlayerState::currEnergy
IL_02c7: ldarg.0
IL_02c8: ldfld System.Int32 PlayerState::maxEnergy
IL_02cd: conv.r4
IL_02ce: bge.un IL_02fc
IL_02d3: ldarg.0
IL_02d4: ldarg.0
IL_02d5: ldfld System.Single PlayerState::energyRecoverAfter
IL_02da: ldarg.0
IL_02db: ldfld TimeDirector PlayerState::timeDir
IL_02e0: callvirt System.Single TimeDirector::WorldTime()
IL_02e5: ldc.r4 60
IL_02ea: ldarg.0
IL_02eb: ldfld System.Single PlayerState::energyRecoveryDelay
IL_02f0: mul
IL_02f1: add
IL_02f2: call System.Single System.Math::Min(System.Single,System.Single)
IL_02f7: stfld System.Single PlayerState::energyRecoverAfter
IL_02fc: nop
IL_02fd: ldarg.0
IL_02fe: ldc.i4.5
IL_02ff: ldc.r4 72
IL_0304: call System.Void PlayerState::MaybeAddUpgradeLock(PlayerState/Upgrade,System.Single)
IL_0309: br IL_0412
IL_030e: ldarg.0
IL_030f: ldarg.0
IL_0310: ldfld System.Int32 PlayerState::maxEnergy
IL_0315: ldarg.0
IL_0316: ldfld System.Int32 PlayerState::defaultMaxEnergy
IL_031b: conv.r4
IL_031c: ldc.r4 2.5
IL_0321: mul
IL_0322: call System.Int32 UnityEngine.Mathf::RoundToInt(System.Single)
IL_0327: call System.Int32 System.Math::Max(System.Int32,System.Int32)
IL_032c: stfld System.Int32 PlayerState::maxEnergy
IL_0331: ldarg.0
IL_0332: ldfld System.Single PlayerState::currEnergy
IL_0337: ldarg.0
IL_0338: ldfld System.Int32 PlayerState::maxEnergy
IL_033d: conv.r4
IL_033e: bge.un IL_036c
IL_0343: ldarg.0
IL_0344: ldarg.0
IL_0345: ldfld System.Single PlayerState::energyRecoverAfter
IL_034a: ldarg.0
IL_034b: ldfld TimeDirector PlayerState::timeDir
IL_0350: callvirt System.Single TimeDirector::WorldTime()
IL_0355: ldc.r4 60
IL_035a: ldarg.0
IL_035b: ldfld System.Single PlayerState::energyRecoveryDelay
IL_0360: mul
IL_0361: add
IL_0362: call System.Single System.Math::Min(System.Single,System.Single)
IL_0367: stfld System.Single PlayerState::energyRecoverAfter
IL_036c: br IL_0412
IL_0371: ldarg.0
IL_0372: ldarg.0
IL_0373: ldfld System.Int32 PlayerState::maxAmmo
IL_0378: ldarg.0
IL_0379: ldfld System.Int32 PlayerState::defaultMaxAmmo
IL_037e: conv.r4
IL_037f: ldc.r4 1.5
IL_0384: mul
IL_0385: call System.Int32 UnityEngine.Mathf::RoundToInt(System.Single)
IL_038a: call System.Int32 System.Math::Max(System.Int32,System.Int32)
IL_038f: stfld System.Int32 PlayerState::maxAmmo
IL_0394: ldarg.0
IL_0395: ldc.i4.7
IL_0396: ldc.r4 48
IL_039b: call System.Void PlayerState::MaybeAddUpgradeLock(PlayerState/Upgrade,System.Single)
IL_03a0: br IL_0412
IL_03a5: ldarg.0
IL_03a6: ldarg.0
IL_03a7: ldfld System.Int32 PlayerState::maxAmmo
IL_03ac: ldarg.0
IL_03ad: ldfld System.Int32 PlayerState::defaultMaxAmmo
IL_03b2: conv.r4
IL_03b3: ldc.r4 2
IL_03b8: mul
IL_03b9: call System.Int32 UnityEngine.Mathf::RoundToInt(System.Single)
IL_03be: call System.Int32 System.Math::Max(System.Int32,System.Int32)
IL_03c3: stfld System.Int32 PlayerState::maxAmmo
IL_03c8: ldarg.0
IL_03c9: ldc.i4.8
IL_03ca: ldc.r4 72
IL_03cf: call System.Void PlayerState::MaybeAddUpgradeLock(PlayerState/Upgrade,System.Single)
IL_03d4: br IL_0412
IL_03d9: ldarg.0
IL_03da: ldarg.0
IL_03db: ldfld System.Int32 PlayerState::maxAmmo
IL_03e0: ldarg.0
IL_03e1: ldfld System.Int32 PlayerState::defaultMaxAmmo
IL_03e6: conv.r4
IL_03e7: ldc.r4 2.5
IL_03ec: mul
IL_03ed: call System.Int32 UnityEngine.Mathf::RoundToInt(System.Single)
IL_03f2: call System.Int32 System.Math::Max(System.Int32,System.Int32)
IL_03f7: stfld System.Int32 PlayerState::maxAmmo
IL_03fc: br IL_0412
IL_0401: ldarg.0
IL_0402: ldfld Ammo PlayerState::ammo
IL_0407: ldc.i4.5
IL_0408: callvirt System.Void Ammo::IncreaseUsableSlots(System.Int32)
IL_040d: br IL_0412
IL_0412: nop
IL_0413: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::Player_ApplyUpgrade
IL_0418: ldarg 
IL_041c: ldloca V_4
IL_0420: ldc.i4 1
IL_0425: newarr System.Object
IL_042a: dup
IL_042b: ldc.i4 0
IL_0430: ldarg upgrade
IL_0434: box PlayerState/Upgrade
IL_0439: stelem.ref
IL_043a: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_043f: pop
IL_0440: ret

*/

// PlayerState
private void ApplyUpgrade(PlayerState.Upgrade upgrade)
{
	object obj = null;
	EnergyJetpack component = SRSingleton<SceneContext>.Instance.Player.GetComponent<EnergyJetpack>();
	StaminaRun component2 = SRSingleton<SceneContext>.Instance.Player.GetComponent<StaminaRun>();
	WeaponVacuum componentInChildren = SRSingleton<SceneContext>.Instance.Player.GetComponentInChildren<WeaponVacuum>();
	switch (upgrade)
	{
	case PlayerState.Upgrade.HEALTH_1:
		this.maxHealth = Math.Max(this.maxHealth, Mathf.RoundToInt((float)this.defaultMaxHealth * 1.5f));
		if (this.currHealth < (float)this.maxHealth)
		{
			this.healthBurstAfter = Math.Min(this.healthBurstAfter, this.timeDir.WorldTime() + 60f * this.healthBurstAmount / this.healthRecoveredPerSecond);
		}
		this.MaybeAddUpgradeLock(PlayerState.Upgrade.HEALTH_2, 48f);
		break;
	case PlayerState.Upgrade.HEALTH_2:
		this.maxHealth = Math.Max(this.maxHealth, Mathf.RoundToInt((float)this.defaultMaxHealth * 2f));
		if (this.currHealth < (float)this.maxHealth)
		{
			this.healthBurstAfter = Math.Min(this.healthBurstAfter, this.timeDir.WorldTime() + 60f * this.healthBurstAmount / this.healthRecoveredPerSecond);
		}
		this.MaybeAddUpgradeLock(PlayerState.Upgrade.HEALTH_3, 72f);
		break;
	case PlayerState.Upgrade.HEALTH_3:
		this.maxHealth = Math.Max(this.maxHealth, Mathf.RoundToInt((float)this.defaultMaxHealth * 2.5f));
		if (this.currHealth < (float)this.maxHealth)
		{
			this.healthBurstAfter = Math.Min(this.healthBurstAfter, this.timeDir.WorldTime() + 60f * this.healthBurstAmount / this.healthRecoveredPerSecond);
		}
		break;
	case PlayerState.Upgrade.ENERGY_1:
		this.maxEnergy = Math.Max(this.maxEnergy, Mathf.RoundToInt((float)this.defaultMaxEnergy * 1.5f));
		if (this.currEnergy < (float)this.maxEnergy)
		{
			this.energyRecoverAfter = Math.Min(this.energyRecoverAfter, this.timeDir.WorldTime() + 60f * this.energyRecoveryDelay);
		}
		this.MaybeAddUpgradeLock(PlayerState.Upgrade.ENERGY_2, 48f);
		break;
	case PlayerState.Upgrade.ENERGY_2:
		this.maxEnergy = Math.Max(this.maxEnergy, Mathf.RoundToInt((float)this.defaultMaxEnergy * 2f));
		if (this.currEnergy < (float)this.maxEnergy)
		{
			this.energyRecoverAfter = Math.Min(this.energyRecoverAfter, this.timeDir.WorldTime() + 60f * this.energyRecoveryDelay);
		}
		this.MaybeAddUpgradeLock(PlayerState.Upgrade.ENERGY_3, 72f);
		break;
	case PlayerState.Upgrade.ENERGY_3:
		this.maxEnergy = Math.Max(this.maxEnergy, Mathf.RoundToInt((float)this.defaultMaxEnergy * 2.5f));
		if (this.currEnergy < (float)this.maxEnergy)
		{
			this.energyRecoverAfter = Math.Min(this.energyRecoverAfter, this.timeDir.WorldTime() + 60f * this.energyRecoveryDelay);
		}
		break;
	case PlayerState.Upgrade.AMMO_1:
		this.maxAmmo = Math.Max(this.maxAmmo, Mathf.RoundToInt((float)this.defaultMaxAmmo * 1.5f));
		this.MaybeAddUpgradeLock(PlayerState.Upgrade.AMMO_2, 48f);
		break;
	case PlayerState.Upgrade.AMMO_2:
		this.maxAmmo = Math.Max(this.maxAmmo, Mathf.RoundToInt((float)this.defaultMaxAmmo * 2f));
		this.MaybeAddUpgradeLock(PlayerState.Upgrade.AMMO_3, 72f);
		break;
	case PlayerState.Upgrade.AMMO_3:
		this.maxAmmo = Math.Max(this.maxAmmo, Mathf.RoundToInt((float)this.defaultMaxAmmo * 2.5f));
		break;
	case PlayerState.Upgrade.JETPACK:
		component.HasJetpack = true;
		this.MaybeAddUpgradeLock(PlayerState.Upgrade.JETPACK_EFFICIENCY, 120f);
		break;
	case PlayerState.Upgrade.JETPACK_EFFICIENCY:
		component.Efficiency = Math.Min(component.Efficiency, 0.8f);
		break;
	case PlayerState.Upgrade.AIR_BURST:
		componentInChildren.HasAirBurst = true;
		break;
	case PlayerState.Upgrade.RUN_EFFICIENCY:
		component2.Efficiency = Math.Min(component2.Efficiency, 0.8333f);
		break;
	case PlayerState.Upgrade.LIQUID_SLOT:
		this.ammo.IncreaseUsableSlots(5);
		break;
	}
	SiscosHooks.call(HOOK_ID.Player_ApplyUpgrade, this, ref obj, new object[]
	{
		upgrade
	});
}

/*
IL_0000: ldc.i4.0
IL_0001: box System.Boolean
IL_0006: stloc V_1
IL_000a: nop
IL_000b: ldarg.0
IL_000c: ldarg.1
IL_000d: call System.Boolean PlayerState::HasUpgrade(PlayerState/Upgrade)
IL_0012: brfalse IL_0019
IL_0017: ldc.i4.0
IL_0018: ret
IL_0019: nop
IL_001a: ldarg.0
IL_001b: ldfld System.Collections.Generic.Dictionary`2<PlayerState/Upgrade,System.Single> PlayerState::upgradeLocks
IL_0020: ldarg.1
IL_0021: callvirt System.Boolean System.Collections.Generic.Dictionary`2<PlayerState/Upgrade,System.Single>::ContainsKey(!0)
IL_0026: brfalse IL_002d
IL_002b: ldc.i4.0
IL_002c: ret
IL_002d: nop
IL_002e: ldarg.1
IL_002f: stloc.0
IL_0030: ldloc.0
IL_0031: ldc.i4.1
IL_0032: sub
IL_0033: switch IL_0065,IL_006d,IL_009e,IL_0075,IL_007d,IL_009e,IL_0085,IL_008d,IL_009e,IL_0095
IL_0060: br IL_009e
IL_0065: ldarg.0
IL_0066: ldc.i4.0
IL_0067: call System.Boolean PlayerState::HasUpgrade(PlayerState/Upgrade)
IL_006c: ret
IL_006d: ldarg.0
IL_006e: ldc.i4.1
IL_006f: call System.Boolean PlayerState::HasUpgrade(PlayerState/Upgrade)
IL_0074: ret
IL_0075: ldarg.0
IL_0076: ldc.i4.3
IL_0077: call System.Boolean PlayerState::HasUpgrade(PlayerState/Upgrade)
IL_007c: ret
IL_007d: ldarg.0
IL_007e: ldc.i4.4
IL_007f: call System.Boolean PlayerState::HasUpgrade(PlayerState/Upgrade)
IL_0084: ret
IL_0085: ldarg.0
IL_0086: ldc.i4.6
IL_0087: call System.Boolean PlayerState::HasUpgrade(PlayerState/Upgrade)
IL_008c: ret
IL_008d: ldarg.0
IL_008e: ldc.i4.7
IL_008f: call System.Boolean PlayerState::HasUpgrade(PlayerState/Upgrade)
IL_0094: ret
IL_0095: ldarg.0
IL_0096: ldc.i4.s 9
IL_0098: call System.Boolean PlayerState::HasUpgrade(PlayerState/Upgrade)
IL_009d: ret
IL_009e: ldc.i4.1
IL_009f: box System.Boolean
IL_00a4: stloc V_1
IL_00a8: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::Player_CanBuyUpgrade
IL_00ad: ldarg 
IL_00b1: ldloca V_1
IL_00b5: ldc.i4 1
IL_00ba: newarr System.Object
IL_00bf: dup
IL_00c0: ldc.i4 0
IL_00c5: ldarg upgrade
IL_00c9: box PlayerState/Upgrade
IL_00ce: stelem.ref
IL_00cf: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_00d4: stloc V_2
IL_00d8: ldloc V_1
IL_00dc: unbox.any System.Boolean
IL_00e1: ret

*/

// PlayerState
public bool CanGetUpgrade(PlayerState.Upgrade upgrade)
{
	object obj = false;
	if (this.HasUpgrade(upgrade))
	{
		return false;
	}
	if (this.upgradeLocks.ContainsKey(upgrade))
	{
		return false;
	}
	switch (upgrade)
	{
	case PlayerState.Upgrade.HEALTH_2:
		return this.HasUpgrade(PlayerState.Upgrade.HEALTH_1);
	case PlayerState.Upgrade.HEALTH_3:
		return this.HasUpgrade(PlayerState.Upgrade.HEALTH_2);
	case PlayerState.Upgrade.ENERGY_2:
		return this.HasUpgrade(PlayerState.Upgrade.ENERGY_1);
	case PlayerState.Upgrade.ENERGY_3:
		return this.HasUpgrade(PlayerState.Upgrade.ENERGY_2);
	case PlayerState.Upgrade.AMMO_2:
		return this.HasUpgrade(PlayerState.Upgrade.AMMO_1);
	case PlayerState.Upgrade.AMMO_3:
		return this.HasUpgrade(PlayerState.Upgrade.AMMO_2);
	case PlayerState.Upgrade.JETPACK_EFFICIENCY:
		return this.HasUpgrade(PlayerState.Upgrade.JETPACK);
	}
	obj = true;
	_hook_result hook_result = SiscosHooks.call(HOOK_ID.Player_CanBuyUpgrade, this, ref obj, new object[]
	{
		upgrade
	});
	return (bool)obj;
}

/*
IL_0000: ldc.i4.0
IL_0001: box System.Boolean
IL_0006: stloc V_0
IL_000a: nop
IL_000b: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::Player_Damaged
IL_0010: ldarg 
IL_0014: ldloca V_0
IL_0018: ldc.i4 1
IL_001d: newarr System.Object
IL_0022: dup
IL_0023: ldc.i4 0
IL_0028: ldarg healthLoss
IL_002c: box System.Int32
IL_0031: stelem.ref
IL_0032: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_0037: stloc V_1
IL_003b: ldloc V_1
IL_003f: ldfld System.Boolean SR_PluginLoader._hook_result::abort
IL_0044: brfalse IL_0053
IL_0049: ldloc V_0
IL_004d: unbox.any System.Boolean
IL_0052: ret
IL_0053: nop
IL_0054: ldloc V_1
IL_0058: ldfld System.Object[] SR_PluginLoader._hook_result::args
IL_005d: brfalse IL_0078
IL_0062: ldloc V_1
IL_0066: ldfld System.Object[] SR_PluginLoader._hook_result::args
IL_006b: ldc.i4 0
IL_0070: ldelem.ref
IL_0071: unbox.any System.Int32
IL_0076: starg.s healthLoss
IL_0078: nop
IL_0079: ldarg.0
IL_007a: dup
IL_007b: ldfld System.Single PlayerState::currHealth
IL_0080: ldarg.1
IL_0081: conv.r4
IL_0082: sub
IL_0083: stfld System.Single PlayerState::currHealth
IL_0088: ldarg.0
IL_0089: ldfld System.Single PlayerState::currHealth
IL_008e: ldc.r4 0
IL_0093: bgt.un IL_00b0
IL_0098: ldarg.0
IL_0099: ldc.r4 0
IL_009e: stfld System.Single PlayerState::currHealth
IL_00a3: ldarg.0
IL_00a4: ldc.r4 Infinity
IL_00a9: stfld System.Single PlayerState::healthBurstAfter
IL_00ae: ldc.i4.1
IL_00af: ret
IL_00b0: nop
IL_00b1: ldarg.0
IL_00b2: ldarg.0
IL_00b3: ldfld TimeDirector PlayerState::timeDir
IL_00b8: callvirt System.Single TimeDirector::WorldTime()
IL_00bd: ldc.r4 60
IL_00c2: ldarg.0
IL_00c3: ldfld System.Single PlayerState::healthBurstAmount
IL_00c8: mul
IL_00c9: ldarg.0
IL_00ca: ldfld System.Single PlayerState::healthRecoveredPerSecond
IL_00cf: div
IL_00d0: add
IL_00d1: stfld System.Single PlayerState::healthBurstAfter
IL_00d6: ldc.i4.0
IL_00d7: ret

*/

// PlayerState
public bool Damage(int healthLoss)
{
	object obj = false;
	_hook_result hook_result = SiscosHooks.call(HOOK_ID.Player_Damaged, this, ref obj, new object[]
	{
		healthLoss
	});
	if (hook_result.abort)
	{
		return (bool)obj;
	}
	if (hook_result.args != null)
	{
		healthLoss = (int)hook_result.args[0];
	}
	this.currHealth -= (float)healthLoss;
	if (this.currHealth <= 0f)
	{
		this.currHealth = 0f;
		this.healthBurstAfter = float.PositiveInfinity;
		return true;
	}
	this.healthBurstAfter = this.timeDir.WorldTime() + 60f * this.healthBurstAmount / this.healthRecoveredPerSecond;
	return false;
}

/*
IL_0000: ldnull
IL_0001: stloc V_0
IL_0005: nop
IL_0006: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::Player_SetEnergy
IL_000b: ldarg 
IL_000f: ldloca V_0
IL_0013: ldc.i4 1
IL_0018: newarr System.Object
IL_001d: dup
IL_001e: ldc.i4 0
IL_0023: ldarg energy
IL_0027: box System.Int32
IL_002c: stelem.ref
IL_002d: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_0032: stloc V_1
IL_0036: ldloc V_1
IL_003a: ldfld System.Boolean SR_PluginLoader._hook_result::abort
IL_003f: brfalse IL_0045
IL_0044: ret
IL_0045: nop
IL_0046: ldloc V_1
IL_004a: ldfld System.Object[] SR_PluginLoader._hook_result::args
IL_004f: brfalse IL_006a
IL_0054: ldloc V_1
IL_0058: ldfld System.Object[] SR_PluginLoader._hook_result::args
IL_005d: ldc.i4 0
IL_0062: ldelem.ref
IL_0063: unbox.any System.Int32
IL_0068: starg.s energy
IL_006a: nop
IL_006b: ldarg.0
IL_006c: ldarg.1
IL_006d: conv.r4
IL_006e: stfld System.Single PlayerState::currEnergy
IL_0073: ldarg.0
IL_0074: ldfld System.Single PlayerState::currEnergy
IL_0079: ldarg.0
IL_007a: ldfld System.Int32 PlayerState::maxEnergy
IL_007f: conv.r4
IL_0080: bge.un IL_00ae
IL_0085: ldarg.0
IL_0086: ldarg.0
IL_0087: ldfld System.Single PlayerState::energyRecoverAfter
IL_008c: ldarg.0
IL_008d: ldfld TimeDirector PlayerState::timeDir
IL_0092: callvirt System.Single TimeDirector::WorldTime()
IL_0097: ldc.r4 60
IL_009c: ldarg.0
IL_009d: ldfld System.Single PlayerState::energyRecoveryDelay
IL_00a2: mul
IL_00a3: add
IL_00a4: call System.Single System.Math::Min(System.Single,System.Single)
IL_00a9: stfld System.Single PlayerState::energyRecoverAfter
IL_00ae: ret

*/

// PlayerState
public void SetEnergy(int energy)
{
	object obj = null;
	_hook_result hook_result = SiscosHooks.call(HOOK_ID.Player_SetEnergy, this, ref obj, new object[]
	{
		energy
	});
	if (hook_result.abort)
	{
		return;
	}
	if (hook_result.args != null)
	{
		energy = (int)hook_result.args[0];
	}
	this.currEnergy = (float)energy;
	if (this.currEnergy < (float)this.maxEnergy)
	{
		this.energyRecoverAfter = Math.Min(this.energyRecoverAfter, this.timeDir.WorldTime() + 60f * this.energyRecoveryDelay);
	}
}

/*
IL_0000: ldnull
IL_0001: stloc V_0
IL_0005: nop
IL_0006: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::Player_LoseEnergy
IL_000b: ldarg 
IL_000f: ldloca V_0
IL_0013: ldc.i4 1
IL_0018: newarr System.Object
IL_001d: dup
IL_001e: ldc.i4 0
IL_0023: ldarg energy
IL_0027: box System.Single
IL_002c: stelem.ref
IL_002d: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_0032: stloc V_1
IL_0036: ldloc V_1
IL_003a: ldfld System.Boolean SR_PluginLoader._hook_result::abort
IL_003f: brfalse IL_0045
IL_0044: ret
IL_0045: nop
IL_0046: ldloc V_1
IL_004a: ldfld System.Object[] SR_PluginLoader._hook_result::args
IL_004f: brfalse IL_006a
IL_0054: ldloc V_1
IL_0058: ldfld System.Object[] SR_PluginLoader._hook_result::args
IL_005d: ldc.i4 0
IL_0062: ldelem.ref
IL_0063: unbox.any System.Single
IL_0068: starg.s energy
IL_006a: nop
IL_006b: ldarg.0
IL_006c: ldc.r4 0
IL_0071: ldarg.0
IL_0072: ldfld System.Single PlayerState::currEnergy
IL_0077: ldarg.1
IL_0078: sub
IL_0079: call System.Single UnityEngine.Mathf::Max(System.Single,System.Single)
IL_007e: stfld System.Single PlayerState::currEnergy
IL_0083: ldarg.0
IL_0084: ldarg.0
IL_0085: ldfld TimeDirector PlayerState::timeDir
IL_008a: callvirt System.Single TimeDirector::WorldTime()
IL_008f: ldc.r4 60
IL_0094: ldarg.0
IL_0095: ldfld System.Single PlayerState::energyRecoveryDelay
IL_009a: mul
IL_009b: add
IL_009c: stfld System.Single PlayerState::energyRecoverAfter
IL_00a1: ret

*/

// PlayerState
public void SpendEnergy(float energy)
{
	object obj = null;
	_hook_result hook_result = SiscosHooks.call(HOOK_ID.Player_LoseEnergy, this, ref obj, new object[]
	{
		energy
	});
	if (hook_result.abort)
	{
		return;
	}
	if (hook_result.args != null)
	{
		energy = (float)hook_result.args[0];
	}
	this.currEnergy = Mathf.Max(0f, this.currEnergy - energy);
	this.energyRecoverAfter = this.timeDir.WorldTime() + 60f * this.energyRecoveryDelay;
}

/*
IL_0000: ldnull
IL_0001: stloc V_2
IL_0005: nop
IL_0006: ldc.i4.1
IL_0007: newarr PurchaseUI/Purchasable
IL_000c: dup
IL_000d: ldc.i4.0
IL_000e: ldstr "ui"
IL_0013: ldstr "b.demolish"
IL_0018: call System.String MessageUtil::Qualify(System.String,System.String)
IL_001d: ldarg.0
IL_001e: ldfld LandPlotUI/PlotPurchaseItem PondUI::demolish
IL_0023: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_0028: ldarg.0
IL_0029: ldfld LandPlotUI/PlotPurchaseItem PondUI::demolish
IL_002e: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_0033: ldstr "ui"
IL_0038: ldstr "m.desc.demolish"
IL_003d: call System.String MessageUtil::Qualify(System.String,System.String)
IL_0042: ldarg.0
IL_0043: ldfld LandPlotUI/PlotPurchaseItem PondUI::demolish
IL_0048: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_004d: ldloca.s V_1
IL_004f: initobj System.Nullable`1<PediaDirector/Id>
IL_0055: ldloc.1
IL_0056: ldarg.0
IL_0057: ldftn System.Void PondUI::Demolish()
IL_005d: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_0062: ldc.i4.1
IL_0063: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_0068: stelem.ref
IL_0069: stloc.0
IL_006a: call T SRSingleton`1<GameContext>::get_Instance()
IL_006f: callvirt UITemplates GameContext::get_UITemplates()
IL_0074: ldarg.0
IL_0075: ldfld UnityEngine.Sprite PondUI::titleIcon
IL_007a: ldstr "t.pond"
IL_007f: ldloc.0
IL_0080: ldarg.0
IL_0081: dup
IL_0082: ldvirtftn System.Void BaseUI::Close()
IL_0088: newobj System.Void PurchaseUI/OnClose::.ctor(System.Object,System.IntPtr)
IL_008d: callvirt UnityEngine.GameObject UITemplates::CreatePurchaseUI(UnityEngine.Sprite,System.String,PurchaseUI/Purchasable[],PurchaseUI/OnClose)
IL_0092: box UnityEngine.GameObject
IL_0097: stloc V_2
IL_009b: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::Ext_Spawn_Plot_Upgrades_UI
IL_00a0: ldarg 
IL_00a4: ldloca V_2
IL_00a8: ldnull
IL_00a9: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_00ae: stloc V_3
IL_00b2: ldloc V_2
IL_00b6: unbox.any UnityEngine.GameObject
IL_00bb: ret

*/

// PondUI
protected override GameObject CreatePurchaseUI()
{
	object obj = null;
	PurchaseUI.Purchasable[] purchasables = new PurchaseUI.Purchasable[]
	{
		new PurchaseUI.Purchasable(MessageUtil.Qualify("ui", "b.demolish"), this.demolish.icon, this.demolish.img, MessageUtil.Qualify("ui", "m.desc.demolish"), this.demolish.cost, null, new UnityAction(this.Demolish), true)
	};
	obj = SRSingleton<GameContext>.Instance.UITemplates.CreatePurchaseUI(this.titleIcon, "t.pond", purchasables, new PurchaseUI.OnClose(this.Close));
	_hook_result hook_result = SiscosHooks.call(HOOK_ID.Ext_Spawn_Plot_Upgrades_UI, this, ref obj, null);
	return (GameObject)obj;
}

/*
IL_0000: ldnull
IL_0001: stloc V_4
IL_0005: nop
IL_0006: ldarg.1
IL_0007: callvirt System.Boolean UnityEngine.Collider::get_isTrigger()
IL_000c: brtrue IL_017a
IL_0011: ldarg.0
IL_0012: ldfld System.Boolean SiloCatcher::allowsInput
IL_0017: brfalse IL_017a
IL_001c: ldarg.1
IL_001d: callvirt UnityEngine.GameObject UnityEngine.Component::get_gameObject()
IL_0022: stloc.0
IL_0023: ldloc.0
IL_0024: callvirt !!0 UnityEngine.GameObject::GetComponent<Identifiable>()
IL_0029: stloc.1
IL_002a: ldloc.0
IL_002b: callvirt !!0 UnityEngine.GameObject::GetComponent<Vacuumable>()
IL_0030: stloc.2
IL_0031: ldloc.1
IL_0032: ldnull
IL_0033: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_0038: brfalse IL_017a
IL_003d: ldarg.0
IL_003e: ldfld System.Collections.Generic.HashSet`1<UnityEngine.GameObject> SiloCatcher::collectedThisFrame
IL_0043: ldloc.0
IL_0044: callvirt System.Boolean System.Collections.Generic.HashSet`1<UnityEngine.GameObject>::Contains(!0)
IL_0049: brtrue IL_017a
IL_004e: ldloc.2
IL_004f: ldnull
IL_0050: call System.Boolean UnityEngine.Object::op_Equality(UnityEngine.Object,UnityEngine.Object)
IL_0055: brtrue IL_0065
IL_005a: ldloc.2
IL_005b: callvirt System.Boolean Vacuumable::isCaptive()
IL_0060: brtrue IL_017a
IL_0065: nop
IL_0066: ldarg.0
IL_0067: ldfld SiloStorage SiloCatcher::storage
IL_006c: ldloc.1
IL_006d: ldarg.0
IL_006e: ldfld System.Int32 SiloCatcher::slotIdx
IL_0073: callvirt System.Boolean SiloStorage::MaybeAddIdentifiable(Identifiable,System.Int32)
IL_0078: brfalse IL_017a
IL_007d: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::Pre_Silo_Input
IL_0082: ldarg 
IL_0086: ldloca V_4
IL_008a: ldc.i4 1
IL_008f: newarr System.Object
IL_0094: dup
IL_0095: ldc.i4 0
IL_009a: ldarg collider
IL_009e: box UnityEngine.Collider
IL_00a3: stelem.ref
IL_00a4: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_00a9: stloc V_5
IL_00ad: ldloc V_5
IL_00b1: ldfld System.Boolean SR_PluginLoader._hook_result::abort
IL_00b6: brfalse IL_00bc
IL_00bb: ret
IL_00bc: nop
IL_00bd: ldloc V_5
IL_00c1: ldfld System.Object[] SR_PluginLoader._hook_result::args
IL_00c6: brfalse IL_00e1
IL_00cb: ldloc V_5
IL_00cf: ldfld System.Object[] SR_PluginLoader._hook_result::args
IL_00d4: ldc.i4 0
IL_00d9: ldelem.ref
IL_00da: unbox.any UnityEngine.Collider
IL_00df: starg.s collider
IL_00e1: nop
IL_00e2: ldarg.0
IL_00e3: ldfld UnityEngine.GameObject SiloCatcher::storeFX
IL_00e8: ldloc.0
IL_00e9: callvirt UnityEngine.Transform UnityEngine.GameObject::get_transform()
IL_00ee: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_position()
IL_00f3: ldloc.0
IL_00f4: callvirt UnityEngine.Transform UnityEngine.GameObject::get_transform()
IL_00f9: callvirt UnityEngine.Quaternion UnityEngine.Transform::get_rotation()
IL_00fe: call UnityEngine.GameObject SRBehaviour::InstantiateDynamic(UnityEngine.GameObject,UnityEngine.Vector3,UnityEngine.Quaternion)
IL_0103: pop
IL_0104: ldarg.0
IL_0105: ldfld System.Collections.Generic.HashSet`1<UnityEngine.GameObject> SiloCatcher::collectedThisFrame
IL_010a: ldloc.0
IL_010b: callvirt System.Boolean System.Collections.Generic.HashSet`1<UnityEngine.GameObject>::Add(!0)
IL_0110: pop
IL_0111: ldloc.0
IL_0112: callvirt !!0 UnityEngine.GameObject::GetComponent<DestroyOnTouching>()
IL_0117: stloc.3
IL_0118: ldloc.3
IL_0119: ldnull
IL_011a: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_011f: brfalse IL_012a
IL_0124: ldloc.3
IL_0125: callvirt System.Void DestroyOnTouching::NoteDestroying()
IL_012a: nop
IL_012b: ldloc.0
IL_012c: call System.Void UnityEngine.Object::Destroy(UnityEngine.Object)
IL_0131: ldarg.0
IL_0132: ldfld SECTR_AudioSource SiloCatcher::scoreAudio
IL_0137: callvirt System.Void SECTR_AudioSource::Play()
IL_013c: ldarg.0
IL_013d: call System.Single UnityEngine.Time::get_time()
IL_0142: ldc.r4 1
IL_0147: add
IL_0148: stfld System.Single SiloCatcher::accelInUntil
IL_014d: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::Post_Silo_Input
IL_0152: ldarg 
IL_0156: ldloca V_4
IL_015a: ldc.i4 1
IL_015f: newarr System.Object
IL_0164: dup
IL_0165: ldc.i4 0
IL_016a: ldarg collider
IL_016e: box UnityEngine.Collider
IL_0173: stelem.ref
IL_0174: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_0179: pop
IL_017a: nop
IL_017b: ret

*/

// SiloCatcher
public void OnTriggerEnter(Collider collider)
{
	object obj = null;
	if (!collider.isTrigger && this.allowsInput)
	{
		GameObject gameObject = collider.gameObject;
		Identifiable component = gameObject.GetComponent<Identifiable>();
		Vacuumable component2 = gameObject.GetComponent<Vacuumable>();
		if (component != null && !this.collectedThisFrame.Contains(gameObject) && (component2 == null || !component2.isCaptive()) && this.storage.MaybeAddIdentifiable(component, this.slotIdx))
		{
			_hook_result hook_result = SiscosHooks.call(HOOK_ID.Pre_Silo_Input, this, ref obj, new object[]
			{
				collider
			});
			if (hook_result.abort)
			{
				return;
			}
			if (hook_result.args != null)
			{
				collider = (Collider)hook_result.args[0];
			}
			SRBehaviour.InstantiateDynamic(this.storeFX, gameObject.transform.position, gameObject.transform.rotation);
			this.collectedThisFrame.Add(gameObject);
			DestroyOnTouching component3 = gameObject.GetComponent<DestroyOnTouching>();
			if (component3 != null)
			{
				component3.NoteDestroying();
			}
			UnityEngine.Object.Destroy(gameObject);
			this.scoreAudio.Play();
			this.accelInUntil = Time.time + 1f;
			SiscosHooks.call(HOOK_ID.Post_Silo_Input, this, ref obj, new object[]
			{
				collider
			});
		}
	}
}

/*
IL_0000: ldnull
IL_0001: stloc V_5
IL_0005: nop
IL_0006: ldarg.0
IL_0007: ldfld System.Boolean SiloCatcher::allowsOutput
IL_000c: brfalse IL_01e2
IL_0011: ldarg.1
IL_0012: callvirt UnityEngine.GameObject UnityEngine.Component::get_gameObject()
IL_0017: callvirt !!0 UnityEngine.GameObject::GetComponentInParent<SiloActivator>()
IL_001c: stloc.0
IL_001d: ldloc.0
IL_001e: ldnull
IL_001f: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_0024: brfalse IL_01e2
IL_0029: ldloc.0
IL_002a: callvirt System.Boolean UnityEngine.Behaviour::get_enabled()
IL_002f: brfalse IL_01e2
IL_0034: call System.Single UnityEngine.Time::get_time()
IL_0039: ldarg.0
IL_003a: ldfld System.Single SiloCatcher::nextEject
IL_003f: ble.un IL_01e2
IL_0044: ldarg.0
IL_0045: ldfld SiloStorage SiloCatcher::storage
IL_004a: callvirt Ammo SiloStorage::get_Ammo()
IL_004f: ldarg.0
IL_0050: ldfld System.Int32 SiloCatcher::slotIdx
IL_0055: callvirt System.Boolean Ammo::SetAmmoSlot(System.Int32)
IL_005a: pop
IL_005b: ldarg.0
IL_005c: ldfld SiloStorage SiloCatcher::storage
IL_0061: callvirt Ammo SiloStorage::get_Ammo()
IL_0066: callvirt System.Boolean Ammo::HasSelectedAmmo()
IL_006b: brfalse IL_01e2
IL_0070: ldarg.1
IL_0071: callvirt UnityEngine.GameObject UnityEngine.Component::get_gameObject()
IL_0076: callvirt UnityEngine.Transform UnityEngine.GameObject::get_transform()
IL_007b: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_position()
IL_0080: ldarg.0
IL_0081: call UnityEngine.Transform UnityEngine.Component::get_transform()
IL_0086: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_position()
IL_008b: call UnityEngine.Vector3 UnityEngine.Vector3::op_Subtraction(UnityEngine.Vector3,UnityEngine.Vector3)
IL_0090: stloc.s V_4
IL_0092: ldloca.s V_4
IL_0094: call UnityEngine.Vector3 UnityEngine.Vector3::get_normalized()
IL_0099: stloc.1
IL_009a: ldarg.0
IL_009b: call UnityEngine.Transform UnityEngine.Component::get_transform()
IL_00a0: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_forward()
IL_00a5: ldloc.1
IL_00a6: call System.Single UnityEngine.Vector3::Angle(UnityEngine.Vector3,UnityEngine.Vector3)
IL_00ab: call System.Single UnityEngine.Mathf::Abs(System.Single)
IL_00b0: ldc.r4 45
IL_00b5: bgt.un IL_01e2
IL_00ba: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::Pre_Silo_Output
IL_00bf: ldarg 
IL_00c3: ldloca V_5
IL_00c7: ldc.i4 1
IL_00cc: newarr System.Object
IL_00d1: dup
IL_00d2: ldc.i4 0
IL_00d7: ldarg collider
IL_00db: box UnityEngine.Collider
IL_00e0: stelem.ref
IL_00e1: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_00e6: stloc V_6
IL_00ea: ldloc V_6
IL_00ee: ldfld System.Boolean SR_PluginLoader._hook_result::abort
IL_00f3: brfalse IL_00f9
IL_00f8: ret
IL_00f9: nop
IL_00fa: ldloc V_6
IL_00fe: ldfld System.Object[] SR_PluginLoader._hook_result::args
IL_0103: brfalse IL_011e
IL_0108: ldloc V_6
IL_010c: ldfld System.Object[] SR_PluginLoader._hook_result::args
IL_0111: ldc.i4 0
IL_0116: ldelem.ref
IL_0117: unbox.any UnityEngine.Collider
IL_011c: starg.s collider
IL_011e: nop
IL_011f: ldarg.0
IL_0120: ldfld SiloStorage SiloCatcher::storage
IL_0125: callvirt Ammo SiloStorage::get_Ammo()
IL_012a: callvirt UnityEngine.GameObject Ammo::GetSelectedStored()
IL_012f: ldarg.0
IL_0130: call UnityEngine.Transform UnityEngine.Component::get_transform()
IL_0135: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_position()
IL_013a: ldloc.1
IL_013b: ldc.r4 1.2
IL_0140: call UnityEngine.Vector3 UnityEngine.Vector3::op_Multiply(UnityEngine.Vector3,System.Single)
IL_0145: call UnityEngine.Vector3 UnityEngine.Vector3::op_Addition(UnityEngine.Vector3,UnityEngine.Vector3)
IL_014a: ldarg.0
IL_014b: call UnityEngine.Transform UnityEngine.Component::get_transform()
IL_0150: callvirt UnityEngine.Quaternion UnityEngine.Transform::get_rotation()
IL_0155: call UnityEngine.GameObject SRBehaviour::InstantiateDynamic(UnityEngine.GameObject,UnityEngine.Vector3,UnityEngine.Quaternion)
IL_015a: stloc.2
IL_015b: ldarg.0
IL_015c: ldfld SiloStorage SiloCatcher::storage
IL_0161: callvirt Ammo SiloStorage::get_Ammo()
IL_0166: ldc.i4.1
IL_0167: callvirt System.Void Ammo::DecrementSelectedAmmo(System.Int32)
IL_016c: ldloc.2
IL_016d: callvirt !!0 UnityEngine.GameObject::GetComponent<Vacuumable>()
IL_0172: stloc.3
IL_0173: ldloc.3
IL_0174: ldnull
IL_0175: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_017a: brfalse IL_018b
IL_017f: ldarg.0
IL_0180: ldfld WeaponVacuum SiloCatcher::vac
IL_0185: ldloc.3
IL_0186: callvirt System.Void WeaponVacuum::ForceJoint(Vacuumable)
IL_018b: nop
IL_018c: ldarg.0
IL_018d: call System.Single UnityEngine.Time::get_time()
IL_0192: ldc.r4 0.25
IL_0197: ldarg.0
IL_0198: ldfld System.Single SiloCatcher::outSpeedFactor
IL_019d: div
IL_019e: add
IL_019f: stfld System.Single SiloCatcher::nextEject
IL_01a4: ldarg.0
IL_01a5: call System.Single UnityEngine.Time::get_time()
IL_01aa: ldc.r4 1
IL_01af: add
IL_01b0: stfld System.Single SiloCatcher::accelOutUntil
IL_01b5: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::Post_Silo_Output
IL_01ba: ldarg 
IL_01be: ldloca V_5
IL_01c2: ldc.i4 1
IL_01c7: newarr System.Object
IL_01cc: dup
IL_01cd: ldc.i4 0
IL_01d2: ldarg collider
IL_01d6: box UnityEngine.Collider
IL_01db: stelem.ref
IL_01dc: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_01e1: pop
IL_01e2: nop
IL_01e3: ret

*/

// SiloCatcher
public void OnTriggerStay(Collider collider)
{
	object obj = null;
	if (this.allowsOutput)
	{
		SiloActivator componentInParent = collider.gameObject.GetComponentInParent<SiloActivator>();
		if (componentInParent != null && componentInParent.enabled && Time.time > this.nextEject)
		{
			this.storage.Ammo.SetAmmoSlot(this.slotIdx);
			if (this.storage.Ammo.HasSelectedAmmo())
			{
				Vector3 normalized = (collider.gameObject.transform.position - base.transform.position).normalized;
				if (Mathf.Abs(Vector3.Angle(base.transform.forward, normalized)) <= 45f)
				{
					_hook_result hook_result = SiscosHooks.call(HOOK_ID.Pre_Silo_Output, this, ref obj, new object[]
					{
						collider
					});
					if (hook_result.abort)
					{
						return;
					}
					if (hook_result.args != null)
					{
						collider = (Collider)hook_result.args[0];
					}
					GameObject gameObject = SRBehaviour.InstantiateDynamic(this.storage.Ammo.GetSelectedStored(), base.transform.position + normalized * 1.2f, base.transform.rotation);
					this.storage.Ammo.DecrementSelectedAmmo(1);
					Vacuumable component = gameObject.GetComponent<Vacuumable>();
					if (component != null)
					{
						this.vac.ForceJoint(component);
					}
					this.nextEject = Time.time + 0.25f / this.outSpeedFactor;
					this.accelOutUntil = Time.time + 1f;
					SiscosHooks.call(HOOK_ID.Post_Silo_Output, this, ref obj, new object[]
					{
						collider
					});
				}
			}
		}
	}
}

/*
IL_0000: ldnull
IL_0001: stloc V_2
IL_0005: nop
IL_0006: ldc.i4.4
IL_0007: newarr PurchaseUI/Purchasable
IL_000c: dup
IL_000d: ldc.i4.0
IL_000e: ldstr "m.upgrade.name.silo.storage2"
IL_0013: ldarg.0
IL_0014: ldfld LandPlotUI/UpgradePurchaseItem SiloUI::storage2
IL_0019: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_001e: ldarg.0
IL_001f: ldfld LandPlotUI/UpgradePurchaseItem SiloUI::storage2
IL_0024: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_0029: ldstr "m.upgrade.desc.silo.storage2"
IL_002e: ldarg.0
IL_002f: ldfld LandPlotUI/UpgradePurchaseItem SiloUI::storage2
IL_0034: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_0039: ldc.i4 4003
IL_003e: newobj System.Void System.Nullable`1<PediaDirector/Id>::.ctor(!0)
IL_0043: ldarg.0
IL_0044: ldftn System.Void SiloUI::UpgradeStorage2()
IL_004a: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_004f: ldarg.0
IL_0050: ldfld LandPlot LandPlotUI::activator
IL_0055: ldc.i4.3
IL_0056: callvirt System.Boolean LandPlot::HasUpgrade(LandPlot/Upgrade)
IL_005b: ldc.i4.0
IL_005c: ceq
IL_005e: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_0063: stelem.ref
IL_0064: dup
IL_0065: ldc.i4.1
IL_0066: ldstr "m.upgrade.name.silo.storage2"
IL_006b: ldarg.0
IL_006c: ldfld LandPlotUI/UpgradePurchaseItem SiloUI::storage3
IL_0071: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_0076: ldarg.0
IL_0077: ldfld LandPlotUI/UpgradePurchaseItem SiloUI::storage3
IL_007c: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_0081: ldstr "m.upgrade.desc.silo.storage2"
IL_0086: ldarg.0
IL_0087: ldfld LandPlotUI/UpgradePurchaseItem SiloUI::storage3
IL_008c: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_0091: ldc.i4 4003
IL_0096: newobj System.Void System.Nullable`1<PediaDirector/Id>::.ctor(!0)
IL_009b: ldarg.0
IL_009c: ldftn System.Void SiloUI::UpgradeStorage3()
IL_00a2: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_00a7: ldarg.0
IL_00a8: ldfld LandPlot LandPlotUI::activator
IL_00ad: ldc.i4.4
IL_00ae: callvirt System.Boolean LandPlot::HasUpgrade(LandPlot/Upgrade)
IL_00b3: brtrue IL_00c6
IL_00b8: ldarg.0
IL_00b9: ldfld LandPlot LandPlotUI::activator
IL_00be: ldc.i4.3
IL_00bf: callvirt System.Boolean LandPlot::HasUpgrade(LandPlot/Upgrade)
IL_00c4: br.s IL_00c8
IL_00c6: nop
IL_00c7: ldc.i4.0
IL_00c8: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_00cd: stelem.ref
IL_00ce: dup
IL_00cf: ldc.i4.2
IL_00d0: ldstr "m.upgrade.name.silo.storage2"
IL_00d5: ldarg.0
IL_00d6: ldfld LandPlotUI/UpgradePurchaseItem SiloUI::storage4
IL_00db: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_00e0: ldarg.0
IL_00e1: ldfld LandPlotUI/UpgradePurchaseItem SiloUI::storage4
IL_00e6: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_00eb: ldstr "m.upgrade.desc.silo.storage2"
IL_00f0: ldarg.0
IL_00f1: ldfld LandPlotUI/UpgradePurchaseItem SiloUI::storage4
IL_00f6: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_00fb: ldc.i4 4003
IL_0100: newobj System.Void System.Nullable`1<PediaDirector/Id>::.ctor(!0)
IL_0105: ldarg.0
IL_0106: ldftn System.Void SiloUI::UpgradeStorage4()
IL_010c: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_0111: ldarg.0
IL_0112: ldfld LandPlot LandPlotUI::activator
IL_0117: ldc.i4.5
IL_0118: callvirt System.Boolean LandPlot::HasUpgrade(LandPlot/Upgrade)
IL_011d: brtrue IL_0130
IL_0122: ldarg.0
IL_0123: ldfld LandPlot LandPlotUI::activator
IL_0128: ldc.i4.4
IL_0129: callvirt System.Boolean LandPlot::HasUpgrade(LandPlot/Upgrade)
IL_012e: br.s IL_0132
IL_0130: nop
IL_0131: ldc.i4.0
IL_0132: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_0137: stelem.ref
IL_0138: dup
IL_0139: ldc.i4.3
IL_013a: ldstr "ui"
IL_013f: ldstr "b.demolish"
IL_0144: call System.String MessageUtil::Qualify(System.String,System.String)
IL_0149: ldarg.0
IL_014a: ldfld LandPlotUI/PlotPurchaseItem SiloUI::demolish
IL_014f: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_0154: ldarg.0
IL_0155: ldfld LandPlotUI/PlotPurchaseItem SiloUI::demolish
IL_015a: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_015f: ldstr "ui"
IL_0164: ldstr "m.desc.demolish"
IL_0169: call System.String MessageUtil::Qualify(System.String,System.String)
IL_016e: ldarg.0
IL_016f: ldfld LandPlotUI/PlotPurchaseItem SiloUI::demolish
IL_0174: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_0179: ldloca.s V_1
IL_017b: initobj System.Nullable`1<PediaDirector/Id>
IL_0181: ldloc.1
IL_0182: ldarg.0
IL_0183: ldftn System.Void SiloUI::Demolish()
IL_0189: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_018e: ldc.i4.1
IL_018f: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_0194: stelem.ref
IL_0195: stloc.0
IL_0196: call T SRSingleton`1<GameContext>::get_Instance()
IL_019b: callvirt UITemplates GameContext::get_UITemplates()
IL_01a0: ldarg.0
IL_01a1: ldfld UnityEngine.Sprite SiloUI::titleIcon
IL_01a6: ldstr "t.silo"
IL_01ab: ldloc.0
IL_01ac: ldarg.0
IL_01ad: dup
IL_01ae: ldvirtftn System.Void BaseUI::Close()
IL_01b4: newobj System.Void PurchaseUI/OnClose::.ctor(System.Object,System.IntPtr)
IL_01b9: callvirt UnityEngine.GameObject UITemplates::CreatePurchaseUI(UnityEngine.Sprite,System.String,PurchaseUI/Purchasable[],PurchaseUI/OnClose)
IL_01be: box UnityEngine.GameObject
IL_01c3: stloc V_2
IL_01c7: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::Ext_Spawn_Plot_Upgrades_UI
IL_01cc: ldarg 
IL_01d0: ldloca V_2
IL_01d4: ldnull
IL_01d5: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_01da: stloc V_3
IL_01de: ldloc V_2
IL_01e2: unbox.any UnityEngine.GameObject
IL_01e7: ret

*/

// SiloUI
protected override GameObject CreatePurchaseUI()
{
	object obj = null;
	PurchaseUI.Purchasable[] purchasables = new PurchaseUI.Purchasable[]
	{
		new PurchaseUI.Purchasable("m.upgrade.name.silo.storage2", this.storage2.icon, this.storage2.img, "m.upgrade.desc.silo.storage2", this.storage2.cost, new PediaDirector.Id?(PediaDirector.Id.SILO), new UnityAction(this.UpgradeStorage2), !this.activator.HasUpgrade(LandPlot.Upgrade.STORAGE2)),
		new PurchaseUI.Purchasable("m.upgrade.name.silo.storage2", this.storage3.icon, this.storage3.img, "m.upgrade.desc.silo.storage2", this.storage3.cost, new PediaDirector.Id?(PediaDirector.Id.SILO), new UnityAction(this.UpgradeStorage3), !this.activator.HasUpgrade(LandPlot.Upgrade.STORAGE3) && this.activator.HasUpgrade(LandPlot.Upgrade.STORAGE2)),
		new PurchaseUI.Purchasable("m.upgrade.name.silo.storage2", this.storage4.icon, this.storage4.img, "m.upgrade.desc.silo.storage2", this.storage4.cost, new PediaDirector.Id?(PediaDirector.Id.SILO), new UnityAction(this.UpgradeStorage4), !this.activator.HasUpgrade(LandPlot.Upgrade.STORAGE4) && this.activator.HasUpgrade(LandPlot.Upgrade.STORAGE3)),
		new PurchaseUI.Purchasable(MessageUtil.Qualify("ui", "b.demolish"), this.demolish.icon, this.demolish.img, MessageUtil.Qualify("ui", "m.desc.demolish"), this.demolish.cost, null, new UnityAction(this.Demolish), true)
	};
	obj = SRSingleton<GameContext>.Instance.UITemplates.CreatePurchaseUI(this.titleIcon, "t.silo", purchasables, new PurchaseUI.OnClose(this.Close));
	_hook_result hook_result = SiscosHooks.call(HOOK_ID.Ext_Spawn_Plot_Upgrades_UI, this, ref obj, null);
	return (GameObject)obj;
}

/*
IL_0000: ldnull
IL_0001: stloc V_0
IL_0005: nop
IL_0006: ldarg.0
IL_0007: ldarg.0
IL_0008: call !!0 UnityEngine.Component::GetComponentInParent<LandPlot>()
IL_000d: stfld LandPlot SpawnResource::landPlot
IL_0012: ldarg.0
IL_0013: ldfld TimeDirector SpawnResource::timeDir
IL_0018: callvirt System.Boolean TimeDirector::IsAtStart()
IL_001d: brfalse IL_002e
IL_0022: ldarg.0
IL_0023: ldc.i4.1
IL_0024: call System.Void SpawnResource::Spawn(System.Boolean)
IL_0029: br IL_0050
IL_002e: nop
IL_002f: ldarg.0
IL_0030: ldfld System.Single SpawnResource::nextSpawnTime
IL_0035: ldc.r4 0
IL_003a: bne.un IL_0050
IL_003f: ldarg.0
IL_0040: ldarg.0
IL_0041: ldfld TimeDirector SpawnResource::timeDir
IL_0046: callvirt System.Single TimeDirector::WorldTime()
IL_004b: stfld System.Single SpawnResource::nextSpawnTime
IL_0050: nop
IL_0051: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::ResourcePatch_Init
IL_0056: ldarg 
IL_005a: ldloca V_0
IL_005e: ldnull
IL_005f: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_0064: pop
IL_0065: ret

*/

// SpawnResource
public void Start()
{
	object obj = null;
	this.landPlot = base.GetComponentInParent<LandPlot>();
	if (this.timeDir.IsAtStart())
	{
		this.Spawn(true);
	}
	else if (this.nextSpawnTime == 0f)
	{
		this.nextSpawnTime = this.timeDir.WorldTime();
	}
	SiscosHooks.call(HOOK_ID.ResourcePatch_Init, this, ref obj, null);
}

/*
IL_0000: ldc.i4.0
IL_0001: box System.Boolean
IL_0006: stloc V_0
IL_000a: nop
IL_000b: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::VacPak_Can_Capture
IL_0010: ldarg 
IL_0014: ldloca V_0
IL_0018: ldnull
IL_0019: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_001e: stloc V_1
IL_0022: ldloc V_1
IL_0026: ldfld System.Boolean SR_PluginLoader._hook_result::abort
IL_002b: brfalse IL_003a
IL_0030: ldloc V_0
IL_0034: unbox.any System.Boolean
IL_0039: ret
IL_003a: nop
IL_003b: call System.Single UnityEngine.Time::get_time()
IL_0040: ldarg.0
IL_0041: ldfld System.Single Vacuumable::nextVacTime
IL_0046: clt.un
IL_0048: ldc.i4.0
IL_0049: ceq
IL_004b: ret

*/

// Vacuumable
public bool canCapture()
{
	object obj = false;
	_hook_result hook_result = SiscosHooks.call(HOOK_ID.VacPak_Can_Capture, this, ref obj, null);
	if (hook_result.abort)
	{
		return (bool)obj;
	}
	return Time.time >= this.nextVacTime;
}

/*
IL_0000: ldnull
IL_0001: stloc V_0
IL_0005: nop
IL_0006: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::VacPak_Capture
IL_000b: ldarg 
IL_000f: ldloca V_0
IL_0013: ldc.i4 1
IL_0018: newarr System.Object
IL_001d: dup
IL_001e: ldc.i4 0
IL_0023: ldarg toJoint
IL_0027: box UnityEngine.Joint
IL_002c: stelem.ref
IL_002d: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_0032: stloc V_1
IL_0036: ldloc V_1
IL_003a: ldfld System.Boolean SR_PluginLoader._hook_result::abort
IL_003f: brfalse IL_0045
IL_0044: ret
IL_0045: nop
IL_0046: ldloc V_1
IL_004a: ldfld System.Object[] SR_PluginLoader._hook_result::args
IL_004f: brfalse IL_006a
IL_0054: ldloc V_1
IL_0058: ldfld System.Object[] SR_PluginLoader._hook_result::args
IL_005d: ldc.i4 0
IL_0062: ldelem.ref
IL_0063: unbox.any UnityEngine.Joint
IL_0068: starg.s toJoint
IL_006a: nop
IL_006b: ldarg.0
IL_006c: ldfld UnityEngine.Joint Vacuumable::captiveToJoint
IL_0071: ldnull
IL_0072: call System.Boolean UnityEngine.Object::op_Equality(UnityEngine.Object,UnityEngine.Object)
IL_0077: brfalse IL_009d
IL_007c: ldarg.0
IL_007d: ldfld SlimeFaceAnimator Vacuumable::sfAnimator
IL_0082: ldnull
IL_0083: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_0088: brfalse IL_009e
IL_008d: ldarg.0
IL_008e: ldfld SlimeFaceAnimator Vacuumable::sfAnimator
IL_0093: ldstr "triggerAwe"
IL_0098: callvirt System.Void SlimeFaceAnimator::SetTrigger(System.String)
IL_009d: nop
IL_009e: nop
IL_009f: ldarg.0
IL_00a0: ldfld UnityEngine.Rigidbody Vacuumable::body
IL_00a5: ldnull
IL_00a6: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_00ab: brfalse IL_00bc
IL_00b0: ldarg.0
IL_00b1: ldfld UnityEngine.Rigidbody Vacuumable::body
IL_00b6: ldc.i4.0
IL_00b7: callvirt System.Void UnityEngine.Rigidbody::set_isKinematic(System.Boolean)
IL_00bc: nop
IL_00bd: ldarg.0
IL_00be: ldarg.1
IL_00bf: call System.Void Vacuumable::SetCaptive(UnityEngine.Joint)
IL_00c4: ret

*/

// Vacuumable
public void capture(Joint toJoint)
{
	object obj = null;
	_hook_result hook_result = SiscosHooks.call(HOOK_ID.VacPak_Capture, this, ref obj, new object[]
	{
		toJoint
	});
	if (hook_result.abort)
	{
		return;
	}
	if (hook_result.args != null)
	{
		toJoint = (Joint)hook_result.args[0];
	}
	if (this.captiveToJoint == null)
	{
		if (this.sfAnimator != null)
		{
			this.sfAnimator.SetTrigger("triggerAwe");
		}
	}
	if (this.body != null)
	{
		this.body.isKinematic = false;
	}
	this.SetCaptive(toJoint);
}

/*
IL_0000: ldnull
IL_0001: stloc V_16
IL_0005: nop
IL_0006: ldarg slimesInVac
IL_000a: ldind.i4
IL_000b: box System.Int32
IL_0010: stloc V_17
IL_0014: ldarg currLiquids
IL_0018: ldind.ref
IL_0019: box System.Collections.Generic.List`1<LiquidSource>
IL_001e: stloc V_18
IL_0022: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::VacPak_Consume
IL_0027: ldarg 
IL_002b: ldloca V_16
IL_002f: ldc.i4 3
IL_0034: newarr System.Object
IL_0039: dup
IL_003a: ldc.i4 0
IL_003f: ldarg gameObj
IL_0043: box UnityEngine.GameObject
IL_0048: stelem.ref
IL_0049: dup
IL_004a: ldc.i4 1
IL_004f: ldloc V_17
IL_0053: stelem.ref
IL_0054: dup
IL_0055: ldc.i4 2
IL_005a: ldloc V_18
IL_005e: stelem.ref
IL_005f: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_0064: stloc V_19
IL_0068: ldloc V_19
IL_006c: ldfld System.Boolean SR_PluginLoader._hook_result::abort
IL_0071: brfalse IL_0077
IL_0076: ret
IL_0077: nop
IL_0078: ldloc V_19
IL_007c: ldfld System.Object[] SR_PluginLoader._hook_result::args
IL_0081: brfalse IL_00ca
IL_0086: ldloc V_19
IL_008a: ldfld System.Object[] SR_PluginLoader._hook_result::args
IL_008f: ldc.i4 0
IL_0094: ldelem.ref
IL_0095: unbox.any UnityEngine.GameObject
IL_009a: starg.s gameObj
IL_009c: ldarg.s slimesInVac
IL_009e: ldloc V_19
IL_00a2: ldfld System.Object[] SR_PluginLoader._hook_result::args
IL_00a7: ldc.i4 1
IL_00ac: ldelem.ref
IL_00ad: unbox.any System.Int32
IL_00b2: stind.i4
IL_00b3: ldarg.s currLiquids
IL_00b5: ldloc V_19
IL_00b9: ldfld System.Object[] SR_PluginLoader._hook_result::args
IL_00be: ldc.i4 2
IL_00c3: ldelem.ref
IL_00c4: unbox.any System.Collections.Generic.List`1<LiquidSource>
IL_00c9: stind.ref
IL_00ca: nop
IL_00cb: ldarg.1
IL_00cc: callvirt !!0 UnityEngine.GameObject::GetComponent<Vacuumable>()
IL_00d1: stloc.0
IL_00d2: ldarg.1
IL_00d3: callvirt !!0 UnityEngine.GameObject::GetComponent<Identifiable>()
IL_00d8: stloc.1
IL_00d9: ldarg.1
IL_00da: callvirt !!0 UnityEngine.GameObject::GetComponent<LiquidSource>()
IL_00df: stloc.2
IL_00e0: ldloc.1
IL_00e1: ldnull
IL_00e2: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_00e7: brfalse IL_0102
IL_00ec: ldloc.1
IL_00ed: ldfld Identifiable/Id Identifiable::id
IL_00f2: call System.Boolean Identifiable::IsSlime(Identifiable/Id)
IL_00f7: brfalse IL_0103
IL_00fc: ldarg.2
IL_00fd: ldarg.2
IL_00fe: ldind.i4
IL_00ff: ldc.i4.1
IL_0100: add
IL_0101: stind.i4
IL_0102: nop
IL_0103: nop
IL_0104: ldloc.0
IL_0105: ldnull
IL_0106: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_010b: brfalse IL_042c
IL_0110: ldloc.0
IL_0111: callvirt System.Boolean UnityEngine.Behaviour::get_enabled()
IL_0116: brfalse IL_042d
IL_011b: ldarg.0
IL_011c: ldfld System.Collections.Generic.HashSet`1<Vacuumable> WeaponVacuum::animatingConsume
IL_0121: ldloc.0
IL_0122: callvirt System.Boolean System.Collections.Generic.HashSet`1<Vacuumable>::Contains(!0)
IL_0127: brtrue IL_042e
IL_012c: ldarg.1
IL_012d: callvirt UnityEngine.Transform UnityEngine.GameObject::get_transform()
IL_0132: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_position()
IL_0137: ldarg.0
IL_0138: ldfld UnityEngine.GameObject WeaponVacuum::vacOrigin
IL_013d: callvirt UnityEngine.Transform UnityEngine.GameObject::get_transform()
IL_0142: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_position()
IL_0147: call UnityEngine.Vector3 UnityEngine.Vector3::op_Subtraction(UnityEngine.Vector3,UnityEngine.Vector3)
IL_014c: stloc.3
IL_014d: ldloca.s V_4
IL_014f: ldarg.0
IL_0150: ldfld UnityEngine.GameObject WeaponVacuum::vacOrigin
IL_0155: callvirt UnityEngine.Transform UnityEngine.GameObject::get_transform()
IL_015a: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_position()
IL_015f: ldloc.3
IL_0160: call System.Void UnityEngine.Ray::.ctor(UnityEngine.Vector3,UnityEngine.Vector3)
IL_0165: ldloc.s V_4
IL_0167: ldloca.s V_5
IL_0169: ldarg.0
IL_016a: ldfld System.Single WeaponVacuum::maxVacDist
IL_016f: ldc.i4 -1610612997
IL_0174: call System.Boolean UnityEngine.Physics::Raycast(UnityEngine.Ray,UnityEngine.RaycastHit&,System.Single,System.Int32)
IL_0179: brfalse IL_042f
IL_017e: ldloca.s V_5
IL_0180: call UnityEngine.Rigidbody UnityEngine.RaycastHit::get_rigidbody()
IL_0185: ldnull
IL_0186: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_018b: brfalse IL_02e4
IL_0190: ldloc.0
IL_0191: callvirt System.Boolean Vacuumable::GetDestroyOnVac()
IL_0196: brfalse IL_01c8
IL_019b: ldarg.0
IL_019c: ldfld UnityEngine.GameObject WeaponVacuum::destroyOnVacFX
IL_01a1: ldarg.1
IL_01a2: callvirt UnityEngine.Transform UnityEngine.GameObject::get_transform()
IL_01a7: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_position()
IL_01ac: ldarg.1
IL_01ad: callvirt UnityEngine.Transform UnityEngine.GameObject::get_transform()
IL_01b2: callvirt UnityEngine.Quaternion UnityEngine.Transform::get_rotation()
IL_01b7: call UnityEngine.GameObject SRBehaviour::InstantiateDynamic(UnityEngine.GameObject,UnityEngine.Vector3,UnityEngine.Quaternion)
IL_01bc: pop
IL_01bd: ldarg.1
IL_01be: call System.Void UnityEngine.Object::Destroy(UnityEngine.Object)
IL_01c3: br IL_02ea
IL_01c8: nop
IL_01c9: ldloc.0
IL_01ca: callvirt System.Boolean Vacuumable::canCapture()
IL_01cf: brfalse IL_02e5
IL_01d4: ldarg.0
IL_01d5: ldfld System.Boolean WeaponVacuum::slimeFilter
IL_01da: brfalse IL_01fb
IL_01df: ldloc.1
IL_01e0: ldnull
IL_01e1: call System.Boolean UnityEngine.Object::op_Equality(UnityEngine.Object,UnityEngine.Object)
IL_01e6: brtrue IL_01fc
IL_01eb: ldloc.1
IL_01ec: ldfld Identifiable/Id Identifiable::id
IL_01f1: call System.Boolean Identifiable::IsSlime(Identifiable/Id)
IL_01f6: brtrue IL_02e6
IL_01fb: nop
IL_01fc: nop
IL_01fd: ldloc.0
IL_01fe: callvirt !!0 UnityEngine.Component::GetComponent<UnityEngine.Rigidbody>()
IL_0203: stloc.s V_6
IL_0205: ldloc.0
IL_0206: callvirt System.Boolean Vacuumable::isCaptive()
IL_020b: brfalse IL_0221
IL_0210: ldloc.0
IL_0211: callvirt System.Boolean Vacuumable::IsTornadoed()
IL_0216: brfalse IL_0222
IL_021b: ldloc.0
IL_021c: callvirt System.Void Vacuumable::release()
IL_0221: nop
IL_0222: nop
IL_0223: ldloc.0
IL_0224: callvirt System.Boolean Vacuumable::isCaptive()
IL_0229: brtrue IL_0283
IL_022e: ldloc.s V_6
IL_0230: callvirt System.Boolean UnityEngine.Rigidbody::get_isKinematic()
IL_0235: brfalse IL_0246
IL_023a: ldloc.0
IL_023b: ldc.i4.1
IL_023c: callvirt System.Void Vacuumable::set_Pending(System.Boolean)
IL_0241: br IL_0284
IL_0246: nop
IL_0247: ldloc.0
IL_0248: ldarg.0
IL_0249: ldarg.0
IL_024a: ldfld UnityEngine.GameObject WeaponVacuum::vacOrigin
IL_024f: callvirt UnityEngine.Transform UnityEngine.GameObject::get_transform()
IL_0254: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_position()
IL_0259: ldarg.0
IL_025a: ldfld UnityEngine.GameObject WeaponVacuum::vacOrigin
IL_025f: callvirt UnityEngine.Transform UnityEngine.GameObject::get_transform()
IL_0264: callvirt UnityEngine.Quaternion UnityEngine.Transform::get_rotation()
IL_0269: call UnityEngine.Vector3 UnityEngine.Vector3::get_up()
IL_026e: call UnityEngine.Vector3 UnityEngine.Quaternion::op_Multiply(UnityEngine.Quaternion,UnityEngine.Vector3)
IL_0273: newobj System.Void UnityEngine.Ray::.ctor(UnityEngine.Vector3,UnityEngine.Vector3)
IL_0278: ldloc.0
IL_0279: call UnityEngine.Joint WeaponVacuum::CreateJoint(UnityEngine.Ray,Vacuumable)
IL_027e: callvirt System.Void Vacuumable::capture(UnityEngine.Joint)
IL_0283: nop
IL_0284: ldloc.s V_6
IL_0286: callvirt System.Boolean UnityEngine.Rigidbody::get_isKinematic()
IL_028b: brtrue IL_02e7
IL_0290: ldloc.1
IL_0291: ldnull
IL_0292: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_0297: brfalse IL_02e8
IL_029c: ldloc.1
IL_029d: ldfld Identifiable/Id Identifiable::id
IL_02a2: call System.Boolean Identifiable::IsAnimal(Identifiable/Id)
IL_02a7: brtrue IL_02bc
IL_02ac: ldloc.1
IL_02ad: ldfld Identifiable/Id Identifiable::id
IL_02b2: call System.Boolean Identifiable::IsSlime(Identifiable/Id)
IL_02b7: brfalse IL_02e9
IL_02bc: nop
IL_02bd: ldarg.0
IL_02be: ldarg.1
IL_02bf: ldarg.0
IL_02c0: ldfld UnityEngine.GameObject WeaponVacuum::heldFaceTowards
IL_02c5: callvirt UnityEngine.Transform UnityEngine.GameObject::get_transform()
IL_02ca: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_position()
IL_02cf: ldarg.1
IL_02d0: callvirt UnityEngine.Transform UnityEngine.GameObject::get_transform()
IL_02d5: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_position()
IL_02da: call UnityEngine.Vector3 UnityEngine.Vector3::op_Subtraction(UnityEngine.Vector3,UnityEngine.Vector3)
IL_02df: call System.Void WeaponVacuum::RotateTowards(UnityEngine.GameObject,UnityEngine.Vector3)
IL_02e4: nop
IL_02e5: nop
IL_02e6: nop
IL_02e7: nop
IL_02e8: nop
IL_02e9: nop
IL_02ea: ldloc.0
IL_02eb: callvirt System.Boolean Vacuumable::isCaptive()
IL_02f0: brfalse IL_0430
IL_02f5: ldarg.1
IL_02f6: callvirt UnityEngine.Transform UnityEngine.GameObject::get_transform()
IL_02fb: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_position()
IL_0300: ldloca.s V_4
IL_0302: call UnityEngine.Vector3 UnityEngine.Ray::get_origin()
IL_0307: call System.Single UnityEngine.Vector3::Distance(UnityEngine.Vector3,UnityEngine.Vector3)
IL_030c: ldarg.0
IL_030d: ldfld System.Single WeaponVacuum::captureDist
IL_0312: bge.un IL_0431
IL_0317: ldloc.1
IL_0318: ldfld Identifiable/Id Identifiable::id
IL_031d: brfalse IL_035d
IL_0322: ldloc.0
IL_0323: callvirt System.Boolean UnityEngine.Behaviour::get_enabled()
IL_0328: brfalse IL_035e
IL_032d: ldloc.0
IL_032e: ldfld Vacuumable/Size Vacuumable::size
IL_0333: brtrue IL_035f
IL_0338: ldloc.0
IL_0339: ldnull
IL_033a: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_033f: brfalse IL_0358
IL_0344: ldarg.0
IL_0345: ldarg.0
IL_0346: ldloc.0
IL_0347: ldloc.1
IL_0348: ldfld Identifiable/Id Identifiable::id
IL_034d: call System.Collections.IEnumerator WeaponVacuum::StartConsuming(Vacuumable,Identifiable/Id)
IL_0352: call UnityEngine.Coroutine UnityEngine.MonoBehaviour::StartCoroutine(System.Collections.IEnumerator)
IL_0357: pop
IL_0358: br IL_0434
IL_035d: nop
IL_035e: nop
IL_035f: nop
IL_0360: ldloc.0
IL_0361: callvirt System.Boolean UnityEngine.Behaviour::get_enabled()
IL_0366: brfalse IL_0432
IL_036b: ldloc.0
IL_036c: callvirt System.Boolean Vacuumable::canCapture()
IL_0371: brfalse IL_0433
IL_0376: ldloc.0
IL_0377: callvirt System.Void Vacuumable::hold()
IL_037c: ldarg.0
IL_037d: ldarg.1
IL_037e: stfld UnityEngine.GameObject WeaponVacuum::held
IL_0383: ldloc.1
IL_0384: ldfld Identifiable/Id Identifiable::id
IL_0389: call System.Boolean Identifiable::IsLargo(Identifiable/Id)
IL_038e: brfalse IL_03a0
IL_0393: ldarg.0
IL_0394: ldfld TutorialDirector WeaponVacuum::tutDir
IL_0399: ldc.i4.s 10
IL_039b: callvirt System.Void TutorialDirector::MaybeShowPopup(TutorialDirector/Id)
IL_03a0: nop
IL_03a1: ldarg.0
IL_03a2: ldarg.0
IL_03a3: ldfld TimeDirector WeaponVacuum::timeDir
IL_03a8: callvirt System.Single TimeDirector::WorldTime()
IL_03ad: stfld System.Single WeaponVacuum::heldStartTime
IL_03b2: ldarg.0
IL_03b3: ldfld UnityEngine.GameObject WeaponVacuum::held
IL_03b8: callvirt !!0 UnityEngine.GameObject::GetComponent<SlimeEat>()
IL_03bd: stloc.s V_7
IL_03bf: ldloc.s V_7
IL_03c1: ldnull
IL_03c2: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_03c7: brfalse IL_03d3
IL_03cc: ldloc.s V_7
IL_03ce: callvirt System.Void SlimeEat::ResetEatClock()
IL_03d3: nop
IL_03d4: ldarg.0
IL_03d5: ldfld UnityEngine.GameObject WeaponVacuum::held
IL_03da: callvirt !!0 UnityEngine.GameObject::GetComponent<TentacleGrapple>()
IL_03df: stloc.s V_8
IL_03e1: ldloc.s V_8
IL_03e3: ldnull
IL_03e4: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_03e9: brfalse IL_03f5
IL_03ee: ldloc.s V_8
IL_03f0: callvirt System.Void TentacleGrapple::Release()
IL_03f5: nop
IL_03f6: ldarg.0
IL_03f7: ldfld PediaDirector WeaponVacuum::pediaDir
IL_03fc: ldloc.1
IL_03fd: ldfld Identifiable/Id Identifiable::id
IL_0402: callvirt System.Void PediaDirector::MaybeShowPopup(Identifiable/Id)
IL_0407: ldarg.0
IL_0408: ldarg.0
IL_0409: ldfld SECTR_AudioCue WeaponVacuum::vacHeldCue
IL_040e: call System.Void WeaponVacuum::PlayTransientAudio(SECTR_AudioCue)
IL_0413: call T SRSingleton`1<SceneContext>::get_Instance()
IL_0418: callvirt UnityEngine.GameObject SceneContext::get_Player()
IL_041d: callvirt !!0 UnityEngine.GameObject::GetComponent<ScreenShaker>()
IL_0422: ldc.r4 1
IL_0427: callvirt System.Void ScreenShaker::ShakeFrontImpact(System.Single)
IL_042c: nop
IL_042d: nop
IL_042e: nop
IL_042f: nop
IL_0430: nop
IL_0431: nop
IL_0432: nop
IL_0433: nop
IL_0434: ldloc.2
IL_0435: ldnull
IL_0436: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_043b: brfalse IL_053f
IL_0440: ldloc.2
IL_0441: callvirt System.Boolean LiquidSource::Available()
IL_0446: brfalse IL_053f
IL_044b: ldarg.0
IL_044c: ldfld vp_FPPlayerEventHandler WeaponVacuum::playerEvents
IL_0451: ldfld vp_Activity vp_PlayerEventHandler::Underwater
IL_0456: callvirt System.Boolean vp_Activity::get_Active()
IL_045b: brfalse IL_046d
IL_0460: ldarg.3
IL_0461: ldind.ref
IL_0462: ldloc.2
IL_0463: callvirt System.Void System.Collections.Generic.List`1<LiquidSource>::Add(!0)
IL_0468: br IL_053f
IL_046d: nop
IL_046e: ldloca.s V_9
IL_0470: ldarg.0
IL_0471: ldfld UnityEngine.GameObject WeaponVacuum::vacOrigin
IL_0476: callvirt UnityEngine.Transform UnityEngine.GameObject::get_transform()
IL_047b: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_position()
IL_0480: ldarg.0
IL_0481: ldfld UnityEngine.GameObject WeaponVacuum::vacOrigin
IL_0486: callvirt UnityEngine.Transform UnityEngine.GameObject::get_transform()
IL_048b: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_up()
IL_0490: call System.Void UnityEngine.Ray::.ctor(UnityEngine.Vector3,UnityEngine.Vector3)
IL_0495: ldc.r4 Infinity
IL_049a: stloc.s V_10
IL_049c: ldc.r4 NaN
IL_04a1: stloc.s V_11
IL_04a3: ldloc.s V_9
IL_04a5: ldarg.0
IL_04a6: ldfld System.Single WeaponVacuum::maxVacDist
IL_04ab: ldc.i4 -1610612997
IL_04b0: ldc.i4.2
IL_04b1: call UnityEngine.RaycastHit[] UnityEngine.Physics::RaycastAll(UnityEngine.Ray,System.Single,System.Int32,UnityEngine.QueryTriggerInteraction)
IL_04b6: stloc.s V_12
IL_04b8: ldloc.s V_12
IL_04ba: stloc.s V_14
IL_04bc: ldc.i4.0
IL_04bd: stloc.s V_15
IL_04bf: br IL_0523
IL_04c4: nop
IL_04c5: ldloc.s V_14
IL_04c7: ldloc.s V_15
IL_04c9: ldelema UnityEngine.RaycastHit
IL_04ce: ldobj UnityEngine.RaycastHit
IL_04d3: stloc.s V_13
IL_04d5: ldloca.s V_13
IL_04d7: call UnityEngine.Collider UnityEngine.RaycastHit::get_collider()
IL_04dc: callvirt UnityEngine.GameObject UnityEngine.Component::get_gameObject()
IL_04e1: ldarg.1
IL_04e2: call System.Boolean UnityEngine.Object::op_Equality(UnityEngine.Object,UnityEngine.Object)
IL_04e7: brfalse IL_04fa
IL_04ec: ldloca.s V_13
IL_04ee: call System.Single UnityEngine.RaycastHit::get_distance()
IL_04f3: stloc.s V_11
IL_04f5: br IL_051d
IL_04fa: nop
IL_04fb: ldloca.s V_13
IL_04fd: call UnityEngine.Collider UnityEngine.RaycastHit::get_collider()
IL_0502: callvirt System.Boolean UnityEngine.Collider::get_isTrigger()
IL_0507: brtrue IL_051c
IL_050c: ldloc.s V_10
IL_050e: ldloca.s V_13
IL_0510: call System.Single UnityEngine.RaycastHit::get_distance()
IL_0515: call System.Single System.Math::Min(System.Single,System.Single)
IL_051a: stloc.s V_10
IL_051c: nop
IL_051d: ldloc.s V_15
IL_051f: ldc.i4.1
IL_0520: add
IL_0521: stloc.s V_15
IL_0523: ldloc.s V_15
IL_0525: ldloc.s V_14
IL_0527: ldlen
IL_0528: conv.i4
IL_0529: blt IL_04c4
IL_052e: ldloc.s V_11
IL_0530: ldloc.s V_10
IL_0532: bgt.un IL_053f
IL_0537: ldarg.3
IL_0538: ldind.ref
IL_0539: ldloc.2
IL_053a: callvirt System.Void System.Collections.Generic.List`1<LiquidSource>::Add(!0)
IL_053f: ret

*/

// WeaponVacuum
private void ConsumeVacItem(GameObject gameObj, ref int slimesInVac, ref List<LiquidSource> currLiquids)
{
	object obj = null;
	object obj2 = slimesInVac;
	object obj3 = currLiquids;
	_hook_result hook_result = SiscosHooks.call(HOOK_ID.VacPak_Consume, this, ref obj, new object[]
	{
		gameObj,
		obj2,
		obj3
	});
	if (hook_result.abort)
	{
		return;
	}
	if (hook_result.args != null)
	{
		gameObj = (GameObject)hook_result.args[0];
		slimesInVac = (int)hook_result.args[1];
		currLiquids = (List<LiquidSource>)hook_result.args[2];
	}
	Vacuumable component = gameObj.GetComponent<Vacuumable>();
	Identifiable component2 = gameObj.GetComponent<Identifiable>();
	LiquidSource component3 = gameObj.GetComponent<LiquidSource>();
	if (component2 != null)
	{
		if (Identifiable.IsSlime(component2.id))
		{
			slimesInVac++;
		}
	}
	if (component != null)
	{
		if (component.enabled)
		{
			if (!this.animatingConsume.Contains(component))
			{
				Vector3 direction = gameObj.transform.position - this.vacOrigin.transform.position;
				Ray ray = new Ray(this.vacOrigin.transform.position, direction);
				RaycastHit raycastHit;
				if (Physics.Raycast(ray, out raycastHit, this.maxVacDist, -1610612997))
				{
					if (raycastHit.rigidbody != null)
					{
						if (component.GetDestroyOnVac())
						{
							SRBehaviour.InstantiateDynamic(this.destroyOnVacFX, gameObj.transform.position, gameObj.transform.rotation);
							UnityEngine.Object.Destroy(gameObj);
						}
						else if (component.canCapture())
						{
							if (this.slimeFilter)
							{
								if (!(component2 == null))
								{
									if (Identifiable.IsSlime(component2.id))
									{
										goto IL_2E6;
									}
								}
							}
							Rigidbody component4 = component.GetComponent<Rigidbody>();
							if (component.isCaptive())
							{
								if (component.IsTornadoed())
								{
									component.release();
								}
							}
							if (!component.isCaptive())
							{
								if (component4.isKinematic)
								{
									component.Pending = true;
								}
								else
								{
									component.capture(this.CreateJoint(new Ray(this.vacOrigin.transform.position, this.vacOrigin.transform.rotation * Vector3.up), component));
								}
							}
							if (!component4.isKinematic)
							{
								if (component2 != null)
								{
									if (Identifiable.IsAnimal(component2.id) || Identifiable.IsSlime(component2.id))
									{
										this.RotateTowards(gameObj, this.heldFaceTowards.transform.position - gameObj.transform.position);
									}
								}
							}
						}
					}
					IL_2E6:
					if (component.isCaptive())
					{
						if (Vector3.Distance(gameObj.transform.position, ray.origin) < this.captureDist)
						{
							if (component2.id != Identifiable.Id.NONE)
							{
								if (component.enabled)
								{
									if (component.size == Vacuumable.Size.NORMAL)
									{
										if (component != null)
										{
											base.StartCoroutine(this.StartConsuming(component, component2.id));
										}
										goto IL_434;
									}
								}
							}
							if (component.enabled)
							{
								if (component.canCapture())
								{
									component.hold();
									this.held = gameObj;
									if (Identifiable.IsLargo(component2.id))
									{
										this.tutDir.MaybeShowPopup(TutorialDirector.Id.LARGO);
									}
									this.heldStartTime = this.timeDir.WorldTime();
									SlimeEat component5 = this.held.GetComponent<SlimeEat>();
									if (component5 != null)
									{
										component5.ResetEatClock();
									}
									TentacleGrapple component6 = this.held.GetComponent<TentacleGrapple>();
									if (component6 != null)
									{
										component6.Release();
									}
									this.pediaDir.MaybeShowPopup(component2.id);
									this.PlayTransientAudio(this.vacHeldCue);
									SRSingleton<SceneContext>.Instance.Player.GetComponent<ScreenShaker>().ShakeFrontImpact(1f);
								}
							}
						}
					}
				}
			}
		}
	}
	IL_434:
	if (component3 != null && component3.Available())
	{
		if (this.playerEvents.Underwater.Active)
		{
			currLiquids.Add(component3);
		}
		else
		{
			Ray ray2 = new Ray(this.vacOrigin.transform.position, this.vacOrigin.transform.up);
			float num = float.PositiveInfinity;
			float num2 = float.NaN;
			RaycastHit[] array = Physics.RaycastAll(ray2, this.maxVacDist, -1610612997, QueryTriggerInteraction.Collide);
			RaycastHit[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				RaycastHit raycastHit2 = array2[i];
				if (raycastHit2.collider.gameObject == gameObj)
				{
					num2 = raycastHit2.distance;
				}
				else if (!raycastHit2.collider.isTrigger)
				{
					num = Math.Min(num, raycastHit2.distance);
				}
			}
			if (num2 <= num)
			{
				currLiquids.Add(component3);
			}
		}
	}
}

/*
IL_0000: ldnull
IL_0001: stloc V_2
IL_0005: nop
IL_0006: ldsfld SR_PluginLoader.HOOK_ID SR_PluginLoader.HOOK_ID::VacPak_Think
IL_000b: ldarg 
IL_000f: ldloca V_2
IL_0013: ldnull
IL_0014: call SR_PluginLoader._hook_result SR_PluginLoader.SiscosHooks::call(SR_PluginLoader.HOOK_ID,System.Object,System.Object&,System.Object[])
IL_0019: stloc V_3
IL_001d: ldloc V_3
IL_0021: ldfld System.Boolean SR_PluginLoader._hook_result::abort
IL_0026: brfalse IL_002c
IL_002b: ret
IL_002c: nop
IL_002d: call System.Single UnityEngine.Time::get_timeScale()
IL_0032: ldc.r4 0
IL_0037: bne.un IL_003d
IL_003c: ret
IL_003d: nop
IL_003e: ldarg.0
IL_003f: ldfld TrackCollisions WeaponVacuum::tracker
IL_0044: callvirt System.Collections.Generic.HashSet`1<UnityEngine.GameObject> TrackCollisions::CurrColliders()
IL_0049: stloc.0
IL_004a: ldarg.0
IL_004b: ldloc.0
IL_004c: call System.Void WeaponVacuum::UpdateHud(System.Collections.Generic.HashSet`1<UnityEngine.GameObject>)
IL_0051: ldarg.0
IL_0052: call System.Void WeaponVacuum::UpdateSlotForInputs()
IL_0057: ldarg.0
IL_0058: call System.Void WeaponVacuum::UpdateVacModeForInputs()
IL_005d: call SRInput/PlayerActions SRInput::get_Actions()
IL_0062: ldfld InControl.PlayerAction SRInput/PlayerActions::attack
IL_0067: callvirt System.Boolean InControl.InputControlBase::get_WasPressed()
IL_006c: brtrue IL_0099
IL_0071: call SRInput/PlayerActions SRInput::get_Actions()
IL_0076: ldfld InControl.PlayerAction SRInput/PlayerActions::vac
IL_007b: callvirt System.Boolean InControl.InputControlBase::get_WasPressed()
IL_0080: brtrue IL_009a
IL_0085: call SRInput/PlayerActions SRInput::get_Actions()
IL_008a: ldfld InControl.PlayerAction SRInput/PlayerActions::burst
IL_008f: callvirt System.Boolean InControl.InputControlBase::get_WasPressed()
IL_0094: brfalse IL_00a2
IL_0099: nop
IL_009a: nop
IL_009b: ldarg.0
IL_009c: ldc.i4.0
IL_009d: stfld System.Boolean WeaponVacuum::launchedHeld
IL_00a2: nop
IL_00a3: ldc.r4 1
IL_00a8: stloc.1
IL_00a9: call System.Single UnityEngine.Time::get_fixedTime()
IL_00ae: ldarg.0
IL_00af: ldfld System.Single WeaponVacuum::nextShot
IL_00b4: blt.un IL_00f7
IL_00b9: ldarg.0
IL_00ba: ldfld System.Boolean WeaponVacuum::launchedHeld
IL_00bf: brtrue IL_00f8
IL_00c4: ldarg.0
IL_00c5: ldfld WeaponVacuum/VacMode WeaponVacuum::vacMode
IL_00ca: ldc.i4.1
IL_00cb: bne.un IL_00f9
IL_00d0: ldarg.0
IL_00d1: ldloc.0
IL_00d2: call System.Void WeaponVacuum::Expel(System.Collections.Generic.HashSet`1<UnityEngine.GameObject>)
IL_00d7: ldarg.0
IL_00d8: call System.Single UnityEngine.Time::get_fixedTime()
IL_00dd: ldarg.0
IL_00de: ldfld System.Single WeaponVacuum::shootCooldown
IL_00e3: ldarg.0
IL_00e4: call System.Single WeaponVacuum::GetShootSpeedFactor()
IL_00e9: div
IL_00ea: add
IL_00eb: stfld System.Single WeaponVacuum::nextShot
IL_00f0: ldarg.0
IL_00f1: call System.Single WeaponVacuum::GetShootSpeedFactor()
IL_00f6: stloc.1
IL_00f7: nop
IL_00f8: nop
IL_00f9: nop
IL_00fa: ldarg.0
IL_00fb: ldfld UnityEngine.Animator WeaponVacuum::vacAnimator
IL_0100: ldnull
IL_0101: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_0106: brfalse IL_0117
IL_010b: ldarg.0
IL_010c: ldfld UnityEngine.Animator WeaponVacuum::vacAnimator
IL_0111: ldloc.1
IL_0112: callvirt System.Void UnityEngine.Animator::set_speed(System.Single)
IL_0117: nop
IL_0118: ldarg.0
IL_0119: ldfld System.Boolean WeaponVacuum::launchedHeld
IL_011e: brtrue IL_0193
IL_0123: ldarg.0
IL_0124: ldfld WeaponVacuum/VacMode WeaponVacuum::vacMode
IL_0129: ldc.i4.2
IL_012a: bne.un IL_0194
IL_012f: ldarg.0
IL_0130: ldfld WeaponVacuum/VacAudioHandler WeaponVacuum::vacAudioHandler
IL_0135: ldc.i4.1
IL_0136: callvirt System.Void WeaponVacuum/VacAudioHandler::SetActive(System.Boolean)
IL_013b: ldarg.0
IL_013c: ldfld UnityEngine.GameObject WeaponVacuum::vacFX
IL_0141: ldarg.0
IL_0142: ldfld UnityEngine.GameObject WeaponVacuum::held
IL_0147: ldnull
IL_0148: call System.Boolean UnityEngine.Object::op_Equality(UnityEngine.Object,UnityEngine.Object)
IL_014d: callvirt System.Void UnityEngine.GameObject::SetActive(System.Boolean)
IL_0152: ldarg.0
IL_0153: ldfld SiloActivator WeaponVacuum::siloActivator
IL_0158: ldarg.0
IL_0159: ldfld UnityEngine.GameObject WeaponVacuum::held
IL_015e: ldnull
IL_015f: call System.Boolean UnityEngine.Object::op_Equality(UnityEngine.Object,UnityEngine.Object)
IL_0164: callvirt System.Void UnityEngine.Behaviour::set_enabled(System.Boolean)
IL_0169: ldarg.0
IL_016a: ldfld UnityEngine.GameObject WeaponVacuum::held
IL_016f: ldnull
IL_0170: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_0175: brfalse IL_0186
IL_017a: ldarg.0
IL_017b: ldloc.0
IL_017c: call System.Void WeaponVacuum::UpdateHeld(System.Collections.Generic.HashSet`1<UnityEngine.GameObject>)
IL_0181: br IL_018e
IL_0186: nop
IL_0187: ldarg.0
IL_0188: ldloc.0
IL_0189: call System.Void WeaponVacuum::Consume(System.Collections.Generic.HashSet`1<UnityEngine.GameObject>)
IL_018e: br IL_019b
IL_0193: nop
IL_0194: nop
IL_0195: ldarg.0
IL_0196: call System.Void WeaponVacuum::ClearVac()
IL_019b: ldarg.0
IL_019c: call System.Void WeaponVacuum::UpdateVacAnimators()
IL_01a1: ret

*/

// WeaponVacuum
public void Update()
{
	object obj = null;
	_hook_result hook_result = SiscosHooks.call(HOOK_ID.VacPak_Think, this, ref obj, null);
	if (hook_result.abort)
	{
		return;
	}
	if (Time.timeScale == 0f)
	{
		return;
	}
	HashSet<GameObject> inVac = this.tracker.CurrColliders();
	this.UpdateHud(inVac);
	this.UpdateSlotForInputs();
	this.UpdateVacModeForInputs();
	if (!SRInput.Actions.attack.WasPressed)
	{
		if (!SRInput.Actions.vac.WasPressed)
		{
			if (!SRInput.Actions.burst.WasPressed)
			{
				goto IL_A2;
			}
		}
	}
	this.launchedHeld = false;
	IL_A2:
	float speed = 1f;
	if (Time.fixedTime >= this.nextShot)
	{
		if (!this.launchedHeld)
		{
			if (this.vacMode == WeaponVacuum.VacMode.SHOOT)
			{
				this.Expel(inVac);
				this.nextShot = Time.fixedTime + this.shootCooldown / this.GetShootSpeedFactor();
				speed = this.GetShootSpeedFactor();
			}
		}
	}
	if (this.vacAnimator != null)
	{
		this.vacAnimator.speed = speed;
	}
	if (!this.launchedHeld)
	{
		if (this.vacMode == WeaponVacuum.VacMode.VAC)
		{
			this.vacAudioHandler.SetActive(true);
			this.vacFX.SetActive(this.held == null);
			this.siloActivator.enabled = (this.held == null);
			if (this.held != null)
			{
				this.UpdateHeld(inVac);
			}
			else
			{
				this.Consume(inVac);
			}
			goto IL_19B;
		}
	}
	this.ClearVac();
	IL_19B:
	this.UpdateVacAnimators();
}

