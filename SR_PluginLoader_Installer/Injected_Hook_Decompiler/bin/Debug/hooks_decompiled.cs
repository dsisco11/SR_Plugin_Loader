// Targeted Assembly DLL: c:\program files (x86)\steam\SteamApps\common\Slime Rancher\SlimeRancher_Data\Managed\Assembly-CSharp.dll
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
			if (!this.sector.Hibernate)
			{
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
							goto IL_1AD;
						}
					}
				}
				if (this.HasTooManySlimes())
				{
					this.Despawn(this.slimes, Mathf.CeilToInt(0.1f * (float)this.targetSlimeCount));
				}
				IL_1AD:
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
						goto IL_2AA;
					}
				}
				if (this.HasTooManyAnimals())
				{
					this.Despawn(this.animals, Mathf.CeilToInt(0.1f * (float)this.targetAnimalCount));
				}
				IL_2AA:
				float arg_2FA_0 = this.nextSpawn;
				_hook_result hook_result2 = SiscosHooks.call(HOOK_ID.Post_Region_Spawn_Cycle, this, ref obj, null);
				if (hook_result2.abort)
				{
					return;
				}
				this.nextSpawn = arg_2FA_0 + this.avgSpawnTimeGameHours * 3600f * this.rand.GetInRange(0.5f, 1.5f);
			}
		}
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
		}
		SiscosHooks.call(HOOK_ID.Economy_Updated, this, ref obj, null);
	}
}


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
	if (component != null)
	{
		if (this.plantableDict.ContainsKey(component.id))
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
		}
		SiscosHooks.call(HOOK_ID.Post_Garden_Set_Type, this, ref obj, new object[]
		{
			col
		});
	}
}


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


// LockOnDeath
public void Update()
{
	object obj = null;
	if (this.locked)
	{
		if (this.timeDir.HasReached(this.unlockWorldTime))
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
			}
			SiscosHooks.call(HOOK_ID.Post_Player_Sleep, this, ref obj, null);
		}
	}
}


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


// SiloCatcher
public void OnTriggerEnter(Collider collider)
{
	object obj = null;
	if (!collider.isTrigger)
	{
		if (this.allowsInput)
		{
			GameObject gameObject = collider.gameObject;
			Identifiable component = gameObject.GetComponent<Identifiable>();
			Vacuumable component2 = gameObject.GetComponent<Vacuumable>();
			if (component != null)
			{
				if (!this.collectedThisFrame.Contains(gameObject))
				{
					if (component2 == null || !component2.isCaptive())
					{
						if (this.storage.MaybeAddIdentifiable(component, this.slotIdx))
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
						}
						SiscosHooks.call(HOOK_ID.Post_Silo_Input, this, ref obj, new object[]
						{
							collider
						});
					}
				}
			}
		}
	}
}


// SiloCatcher
public void OnTriggerStay(Collider collider)
{
	object obj = null;
	if (this.allowsOutput)
	{
		SiloActivator componentInParent = collider.gameObject.GetComponentInParent<SiloActivator>();
		if (componentInParent != null)
		{
			if (componentInParent.enabled)
			{
				if (Time.time > this.nextEject)
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
						}
						SiscosHooks.call(HOOK_ID.Post_Silo_Output, this, ref obj, new object[]
						{
							collider
						});
					}
				}
			}
		}
	}
}


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


