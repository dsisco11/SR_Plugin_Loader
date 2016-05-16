// Beginning complete dump of all targeted IL code...
//  WeaponVacuum.ConsumeVacItem
/*
IL_0000: ldarg.1
IL_0001: callvirt !!0 UnityEngine.GameObject::GetComponent<Vacuumable>()
IL_0006: stloc.0
IL_0007: ldarg.1
IL_0008: callvirt !!0 UnityEngine.GameObject::GetComponent<Identifiable>()
IL_000d: stloc.1
IL_000e: ldarg.1
IL_000f: callvirt !!0 UnityEngine.GameObject::GetComponent<LiquidSource>()
IL_0014: stloc.2
IL_0015: ldloc.1
IL_0016: ldnull
IL_0017: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_001c: brfalse IL_0037
IL_0021: ldloc.1
IL_0022: ldfld Identifiable/Id Identifiable::id
IL_0027: call System.Boolean Identifiable::IsSlime(Identifiable/Id)
IL_002c: brfalse IL_0037
IL_0031: ldarg.2
IL_0032: ldarg.2
IL_0033: ldind.i4
IL_0034: ldc.i4.1
IL_0035: add
IL_0036: stind.i4
IL_0037: ldloc.0
IL_0038: ldnull
IL_0039: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_003e: brfalse IL_034b
IL_0043: ldloc.0
IL_0044: callvirt System.Boolean UnityEngine.Behaviour::get_enabled()
IL_0049: brfalse IL_034b
IL_004e: ldarg.0
IL_004f: ldfld System.Collections.Generic.HashSet`1<Vacuumable> WeaponVacuum::animatingConsume
IL_0054: ldloc.0
IL_0055: callvirt System.Boolean System.Collections.Generic.HashSet`1<Vacuumable>::Contains(!0)
IL_005a: brtrue IL_034b
IL_005f: ldarg.1
IL_0060: callvirt UnityEngine.Transform UnityEngine.GameObject::get_transform()
IL_0065: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_position()
IL_006a: ldarg.0
IL_006b: ldfld UnityEngine.GameObject WeaponVacuum::vacOrigin
IL_0070: callvirt UnityEngine.Transform UnityEngine.GameObject::get_transform()
IL_0075: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_position()
IL_007a: call UnityEngine.Vector3 UnityEngine.Vector3::op_Subtraction(UnityEngine.Vector3,UnityEngine.Vector3)
IL_007f: stloc.3
IL_0080: ldloca.s V_4
IL_0082: ldarg.0
IL_0083: ldfld UnityEngine.GameObject WeaponVacuum::vacOrigin
IL_0088: callvirt UnityEngine.Transform UnityEngine.GameObject::get_transform()
IL_008d: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_position()
IL_0092: ldloc.3
IL_0093: call System.Void UnityEngine.Ray::.ctor(UnityEngine.Vector3,UnityEngine.Vector3)
IL_0098: ldloc.s V_4
IL_009a: ldloca.s V_5
IL_009c: ldarg.0
IL_009d: ldfld System.Single WeaponVacuum::maxVacDist
IL_00a2: ldc.i4 -1610612997
IL_00a7: call System.Boolean UnityEngine.Physics::Raycast(UnityEngine.Ray,UnityEngine.RaycastHit&,System.Single,System.Int32)
IL_00ac: brfalse IL_034b
IL_00b1: ldloca.s V_5
IL_00b3: call UnityEngine.Rigidbody UnityEngine.RaycastHit::get_rigidbody()
IL_00b8: ldnull
IL_00b9: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_00be: brfalse IL_020f
IL_00c3: ldloc.0
IL_00c4: callvirt System.Boolean Vacuumable::GetDestroyOnVac()
IL_00c9: brfalse IL_00fb
IL_00ce: ldarg.0
IL_00cf: ldfld UnityEngine.GameObject WeaponVacuum::destroyOnVacFX
IL_00d4: ldarg.1
IL_00d5: callvirt UnityEngine.Transform UnityEngine.GameObject::get_transform()
IL_00da: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_position()
IL_00df: ldarg.1
IL_00e0: callvirt UnityEngine.Transform UnityEngine.GameObject::get_transform()
IL_00e5: callvirt UnityEngine.Quaternion UnityEngine.Transform::get_rotation()
IL_00ea: call UnityEngine.GameObject SRBehaviour::InstantiateDynamic(UnityEngine.GameObject,UnityEngine.Vector3,UnityEngine.Quaternion)
IL_00ef: pop
IL_00f0: ldarg.1
IL_00f1: call System.Void UnityEngine.Object::Destroy(UnityEngine.Object)
IL_00f6: br IL_020f
IL_00fb: ldloc.0
IL_00fc: callvirt System.Boolean Vacuumable::canCapture()
IL_0101: brfalse IL_020f
IL_0106: ldarg.0
IL_0107: ldfld System.Boolean WeaponVacuum::slimeFilter
IL_010c: brfalse IL_012d
IL_0111: ldloc.1
IL_0112: ldnull
IL_0113: call System.Boolean UnityEngine.Object::op_Equality(UnityEngine.Object,UnityEngine.Object)
IL_0118: brtrue IL_012d
IL_011d: ldloc.1
IL_011e: ldfld Identifiable/Id Identifiable::id
IL_0123: call System.Boolean Identifiable::IsSlime(Identifiable/Id)
IL_0128: brtrue IL_020f
IL_012d: ldloc.0
IL_012e: callvirt !!0 UnityEngine.Component::GetComponent<UnityEngine.Rigidbody>()
IL_0133: stloc.s V_6
IL_0135: ldloc.0
IL_0136: callvirt System.Boolean Vacuumable::isCaptive()
IL_013b: brfalse IL_0151
IL_0140: ldloc.0
IL_0141: callvirt System.Boolean Vacuumable::IsTornadoed()
IL_0146: brfalse IL_0151
IL_014b: ldloc.0
IL_014c: callvirt System.Void Vacuumable::release()
IL_0151: ldloc.0
IL_0152: callvirt System.Boolean Vacuumable::isCaptive()
IL_0157: brtrue IL_01b0
IL_015c: ldloc.s V_6
IL_015e: callvirt System.Boolean UnityEngine.Rigidbody::get_isKinematic()
IL_0163: brfalse IL_0174
IL_0168: ldloc.0
IL_0169: ldc.i4.1
IL_016a: callvirt System.Void Vacuumable::set_Pending(System.Boolean)
IL_016f: br IL_01b0
IL_0174: ldloc.0
IL_0175: ldarg.0
IL_0176: ldarg.0
IL_0177: ldfld UnityEngine.GameObject WeaponVacuum::vacOrigin
IL_017c: callvirt UnityEngine.Transform UnityEngine.GameObject::get_transform()
IL_0181: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_position()
IL_0186: ldarg.0
IL_0187: ldfld UnityEngine.GameObject WeaponVacuum::vacOrigin
IL_018c: callvirt UnityEngine.Transform UnityEngine.GameObject::get_transform()
IL_0191: callvirt UnityEngine.Quaternion UnityEngine.Transform::get_rotation()
IL_0196: call UnityEngine.Vector3 UnityEngine.Vector3::get_up()
IL_019b: call UnityEngine.Vector3 UnityEngine.Quaternion::op_Multiply(UnityEngine.Quaternion,UnityEngine.Vector3)
IL_01a0: newobj System.Void UnityEngine.Ray::.ctor(UnityEngine.Vector3,UnityEngine.Vector3)
IL_01a5: ldloc.0
IL_01a6: call UnityEngine.Joint WeaponVacuum::CreateJoint(UnityEngine.Ray,Vacuumable)
IL_01ab: callvirt System.Void Vacuumable::capture(UnityEngine.Joint)
IL_01b0: ldloc.s V_6
IL_01b2: callvirt System.Boolean UnityEngine.Rigidbody::get_isKinematic()
IL_01b7: brtrue IL_020f
IL_01bc: ldloc.1
IL_01bd: ldnull
IL_01be: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_01c3: brfalse IL_020f
IL_01c8: ldloc.1
IL_01c9: ldfld Identifiable/Id Identifiable::id
IL_01ce: call System.Boolean Identifiable::IsAnimal(Identifiable/Id)
IL_01d3: brtrue IL_01e8
IL_01d8: ldloc.1
IL_01d9: ldfld Identifiable/Id Identifiable::id
IL_01de: call System.Boolean Identifiable::IsSlime(Identifiable/Id)
IL_01e3: brfalse IL_020f
IL_01e8: ldarg.0
IL_01e9: ldarg.1
IL_01ea: ldarg.0
IL_01eb: ldfld UnityEngine.GameObject WeaponVacuum::heldFaceTowards
IL_01f0: callvirt UnityEngine.Transform UnityEngine.GameObject::get_transform()
IL_01f5: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_position()
IL_01fa: ldarg.1
IL_01fb: callvirt UnityEngine.Transform UnityEngine.GameObject::get_transform()
IL_0200: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_position()
IL_0205: call UnityEngine.Vector3 UnityEngine.Vector3::op_Subtraction(UnityEngine.Vector3,UnityEngine.Vector3)
IL_020a: call System.Void WeaponVacuum::RotateTowards(UnityEngine.GameObject,UnityEngine.Vector3)
IL_020f: ldloc.0
IL_0210: callvirt System.Boolean Vacuumable::isCaptive()
IL_0215: brfalse IL_034b
IL_021a: ldarg.1
IL_021b: callvirt UnityEngine.Transform UnityEngine.GameObject::get_transform()
IL_0220: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_position()
IL_0225: ldloca.s V_4
IL_0227: call UnityEngine.Vector3 UnityEngine.Ray::get_origin()
IL_022c: call System.Single UnityEngine.Vector3::Distance(UnityEngine.Vector3,UnityEngine.Vector3)
IL_0231: ldarg.0
IL_0232: ldfld System.Single WeaponVacuum::captureDist
IL_0237: bge.un IL_034b
IL_023c: ldloc.1
IL_023d: ldfld Identifiable/Id Identifiable::id
IL_0242: brfalse IL_0282
IL_0247: ldloc.0
IL_0248: callvirt System.Boolean UnityEngine.Behaviour::get_enabled()
IL_024d: brfalse IL_0282
IL_0252: ldloc.0
IL_0253: ldfld Vacuumable/Size Vacuumable::size
IL_0258: brtrue IL_0282
IL_025d: ldloc.0
IL_025e: ldnull
IL_025f: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_0264: brfalse IL_027d
IL_0269: ldarg.0
IL_026a: ldarg.0
IL_026b: ldloc.0
IL_026c: ldloc.1
IL_026d: ldfld Identifiable/Id Identifiable::id
IL_0272: call System.Collections.IEnumerator WeaponVacuum::StartConsuming(Vacuumable,Identifiable/Id)
IL_0277: call UnityEngine.Coroutine UnityEngine.MonoBehaviour::StartCoroutine(System.Collections.IEnumerator)
IL_027c: pop
IL_027d: br IL_034b
IL_0282: ldloc.0
IL_0283: callvirt System.Boolean UnityEngine.Behaviour::get_enabled()
IL_0288: brfalse IL_034b
IL_028d: ldloc.0
IL_028e: callvirt System.Boolean Vacuumable::canCapture()
IL_0293: brfalse IL_034b
IL_0298: ldloc.0
IL_0299: callvirt System.Void Vacuumable::hold()
IL_029e: ldarg.0
IL_029f: ldarg.1
IL_02a0: stfld UnityEngine.GameObject WeaponVacuum::held
IL_02a5: ldloc.1
IL_02a6: ldfld Identifiable/Id Identifiable::id
IL_02ab: call System.Boolean Identifiable::IsLargo(Identifiable/Id)
IL_02b0: brfalse IL_02c2
IL_02b5: ldarg.0
IL_02b6: ldfld TutorialDirector WeaponVacuum::tutDir
IL_02bb: ldc.i4.s 10
IL_02bd: callvirt System.Void TutorialDirector::MaybeShowPopup(TutorialDirector/Id)
IL_02c2: ldarg.0
IL_02c3: ldarg.0
IL_02c4: ldfld TimeDirector WeaponVacuum::timeDir
IL_02c9: callvirt System.Single TimeDirector::WorldTime()
IL_02ce: stfld System.Single WeaponVacuum::heldStartTime
IL_02d3: ldarg.0
IL_02d4: ldfld UnityEngine.GameObject WeaponVacuum::held
IL_02d9: callvirt !!0 UnityEngine.GameObject::GetComponent<SlimeEat>()
IL_02de: stloc.s V_7
IL_02e0: ldloc.s V_7
IL_02e2: ldnull
IL_02e3: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_02e8: brfalse IL_02f4
IL_02ed: ldloc.s V_7
IL_02ef: callvirt System.Void SlimeEat::ResetEatClock()
IL_02f4: ldarg.0
IL_02f5: ldfld UnityEngine.GameObject WeaponVacuum::held
IL_02fa: callvirt !!0 UnityEngine.GameObject::GetComponent<TentacleGrapple>()
IL_02ff: stloc.s V_8
IL_0301: ldloc.s V_8
IL_0303: ldnull
IL_0304: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_0309: brfalse IL_0315
IL_030e: ldloc.s V_8
IL_0310: callvirt System.Void TentacleGrapple::Release()
IL_0315: ldarg.0
IL_0316: ldfld PediaDirector WeaponVacuum::pediaDir
IL_031b: ldloc.1
IL_031c: ldfld Identifiable/Id Identifiable::id
IL_0321: callvirt System.Void PediaDirector::MaybeShowPopup(Identifiable/Id)
IL_0326: ldarg.0
IL_0327: ldarg.0
IL_0328: ldfld SECTR_AudioCue WeaponVacuum::vacHeldCue
IL_032d: call System.Void WeaponVacuum::PlayTransientAudio(SECTR_AudioCue)
IL_0332: call T SRSingleton`1<SceneContext>::get_Instance()
IL_0337: callvirt UnityEngine.GameObject SceneContext::get_Player()
IL_033c: callvirt !!0 UnityEngine.GameObject::GetComponent<ScreenShaker>()
IL_0341: ldc.r4 1
IL_0346: callvirt System.Void ScreenShaker::ShakeFrontImpact(System.Single)
IL_034b: ldloc.2
IL_034c: ldnull
IL_034d: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_0352: brfalse IL_0452
IL_0357: ldloc.2
IL_0358: callvirt System.Boolean LiquidSource::Available()
IL_035d: brfalse IL_0452
IL_0362: ldarg.0
IL_0363: ldfld vp_FPPlayerEventHandler WeaponVacuum::playerEvents
IL_0368: ldfld vp_Activity vp_PlayerEventHandler::Underwater
IL_036d: callvirt System.Boolean vp_Activity::get_Active()
IL_0372: brfalse IL_0384
IL_0377: ldarg.3
IL_0378: ldind.ref
IL_0379: ldloc.2
IL_037a: callvirt System.Void System.Collections.Generic.List`1<LiquidSource>::Add(!0)
IL_037f: br IL_0452
IL_0384: ldloca.s V_9
IL_0386: ldarg.0
IL_0387: ldfld UnityEngine.GameObject WeaponVacuum::vacOrigin
IL_038c: callvirt UnityEngine.Transform UnityEngine.GameObject::get_transform()
IL_0391: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_position()
IL_0396: ldarg.0
IL_0397: ldfld UnityEngine.GameObject WeaponVacuum::vacOrigin
IL_039c: callvirt UnityEngine.Transform UnityEngine.GameObject::get_transform()
IL_03a1: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_up()
IL_03a6: call System.Void UnityEngine.Ray::.ctor(UnityEngine.Vector3,UnityEngine.Vector3)
IL_03ab: ldc.r4 Infinity
IL_03b0: stloc.s V_10
IL_03b2: ldc.r4 NaN
IL_03b7: stloc.s V_11
IL_03b9: ldloc.s V_9
IL_03bb: ldarg.0
IL_03bc: ldfld System.Single WeaponVacuum::maxVacDist
IL_03c1: ldc.i4 -1610612997
IL_03c6: ldc.i4.2
IL_03c7: call UnityEngine.RaycastHit[] UnityEngine.Physics::RaycastAll(UnityEngine.Ray,System.Single,System.Int32,UnityEngine.QueryTriggerInteraction)
IL_03cc: stloc.s V_12
IL_03ce: ldloc.s V_12
IL_03d0: stloc.s V_14
IL_03d2: ldc.i4.0
IL_03d3: stloc.s V_15
IL_03d5: br IL_0436
IL_03da: ldloc.s V_14
IL_03dc: ldloc.s V_15
IL_03de: ldelema UnityEngine.RaycastHit
IL_03e3: ldobj UnityEngine.RaycastHit
IL_03e8: stloc.s V_13
IL_03ea: ldloca.s V_13
IL_03ec: call UnityEngine.Collider UnityEngine.RaycastHit::get_collider()
IL_03f1: callvirt UnityEngine.GameObject UnityEngine.Component::get_gameObject()
IL_03f6: ldarg.1
IL_03f7: call System.Boolean UnityEngine.Object::op_Equality(UnityEngine.Object,UnityEngine.Object)
IL_03fc: brfalse IL_040f
IL_0401: ldloca.s V_13
IL_0403: call System.Single UnityEngine.RaycastHit::get_distance()
IL_0408: stloc.s V_11
IL_040a: br IL_0430
IL_040f: ldloca.s V_13
IL_0411: call UnityEngine.Collider UnityEngine.RaycastHit::get_collider()
IL_0416: callvirt System.Boolean UnityEngine.Collider::get_isTrigger()
IL_041b: brtrue IL_0430
IL_0420: ldloc.s V_10
IL_0422: ldloca.s V_13
IL_0424: call System.Single UnityEngine.RaycastHit::get_distance()
IL_0429: call System.Single System.Math::Min(System.Single,System.Single)
IL_042e: stloc.s V_10
IL_0430: ldloc.s V_15
IL_0432: ldc.i4.1
IL_0433: add
IL_0434: stloc.s V_15
IL_0436: ldloc.s V_15
IL_0438: ldloc.s V_14
IL_043a: ldlen
IL_043b: conv.i4
IL_043c: blt IL_03da
IL_0441: ldloc.s V_11
IL_0443: ldloc.s V_10
IL_0445: bgt.un IL_0452
IL_044a: ldarg.3
IL_044b: ldind.ref
IL_044c: ldloc.2
IL_044d: callvirt System.Void System.Collections.Generic.List`1<LiquidSource>::Add(!0)
IL_0452: ret

*/
//  Vacuumable.canCapture
/*
IL_0000: call System.Single UnityEngine.Time::get_time()
IL_0005: ldarg.0
IL_0006: ldfld System.Single Vacuumable::nextVacTime
IL_000b: clt.un
IL_000d: ldc.i4.0
IL_000e: ceq
IL_0010: ret

*/
//  Vacuumable.capture
/*
IL_0000: ldarg.0
IL_0001: ldfld UnityEngine.Joint Vacuumable::captiveToJoint
IL_0006: ldnull
IL_0007: call System.Boolean UnityEngine.Object::op_Equality(UnityEngine.Object,UnityEngine.Object)
IL_000c: brfalse IL_0032
IL_0011: ldarg.0
IL_0012: ldfld SlimeFaceAnimator Vacuumable::sfAnimator
IL_0017: ldnull
IL_0018: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_001d: brfalse IL_0032
IL_0022: ldarg.0
IL_0023: ldfld SlimeFaceAnimator Vacuumable::sfAnimator
IL_0028: ldstr "triggerAwe"
IL_002d: callvirt System.Void SlimeFaceAnimator::SetTrigger(System.String)
IL_0032: ldarg.0
IL_0033: ldfld UnityEngine.Rigidbody Vacuumable::body
IL_0038: ldnull
IL_0039: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_003e: brfalse IL_004f
IL_0043: ldarg.0
IL_0044: ldfld UnityEngine.Rigidbody Vacuumable::body
IL_0049: ldc.i4.0
IL_004a: callvirt System.Void UnityEngine.Rigidbody::set_isKinematic(System.Boolean)
IL_004f: ldarg.0
IL_0050: ldarg.1
IL_0051: call System.Void Vacuumable::SetCaptive(UnityEngine.Joint)
IL_0056: ret

*/
//  WeaponVacuum.Update
/*
IL_0000: call System.Single UnityEngine.Time::get_timeScale()
IL_0005: ldc.r4 0
IL_000a: bne.un IL_0010
IL_000f: ret
IL_0010: ldarg.0
IL_0011: ldfld TrackCollisions WeaponVacuum::tracker
IL_0016: callvirt System.Collections.Generic.HashSet`1<UnityEngine.GameObject> TrackCollisions::CurrColliders()
IL_001b: stloc.0
IL_001c: ldarg.0
IL_001d: ldloc.0
IL_001e: call System.Void WeaponVacuum::UpdateHud(System.Collections.Generic.HashSet`1<UnityEngine.GameObject>)
IL_0023: ldarg.0
IL_0024: call System.Void WeaponVacuum::UpdateSlotForInputs()
IL_0029: ldarg.0
IL_002a: call System.Void WeaponVacuum::UpdateVacModeForInputs()
IL_002f: call SRInput/PlayerActions SRInput::get_Actions()
IL_0034: ldfld InControl.PlayerAction SRInput/PlayerActions::attack
IL_0039: callvirt System.Boolean InControl.InputControlBase::get_WasPressed()
IL_003e: brtrue IL_006b
IL_0043: call SRInput/PlayerActions SRInput::get_Actions()
IL_0048: ldfld InControl.PlayerAction SRInput/PlayerActions::vac
IL_004d: callvirt System.Boolean InControl.InputControlBase::get_WasPressed()
IL_0052: brtrue IL_006b
IL_0057: call SRInput/PlayerActions SRInput::get_Actions()
IL_005c: ldfld InControl.PlayerAction SRInput/PlayerActions::burst
IL_0061: callvirt System.Boolean InControl.InputControlBase::get_WasPressed()
IL_0066: brfalse IL_0072
IL_006b: ldarg.0
IL_006c: ldc.i4.0
IL_006d: stfld System.Boolean WeaponVacuum::launchedHeld
IL_0072: ldc.r4 1
IL_0077: stloc.1
IL_0078: call System.Single UnityEngine.Time::get_fixedTime()
IL_007d: ldarg.0
IL_007e: ldfld System.Single WeaponVacuum::nextShot
IL_0083: blt.un IL_00c6
IL_0088: ldarg.0
IL_0089: ldfld System.Boolean WeaponVacuum::launchedHeld
IL_008e: brtrue IL_00c6
IL_0093: ldarg.0
IL_0094: ldfld WeaponVacuum/VacMode WeaponVacuum::vacMode
IL_0099: ldc.i4.1
IL_009a: bne.un IL_00c6
IL_009f: ldarg.0
IL_00a0: ldloc.0
IL_00a1: call System.Void WeaponVacuum::Expel(System.Collections.Generic.HashSet`1<UnityEngine.GameObject>)
IL_00a6: ldarg.0
IL_00a7: call System.Single UnityEngine.Time::get_fixedTime()
IL_00ac: ldarg.0
IL_00ad: ldfld System.Single WeaponVacuum::shootCooldown
IL_00b2: ldarg.0
IL_00b3: call System.Single WeaponVacuum::GetShootSpeedFactor()
IL_00b8: div
IL_00b9: add
IL_00ba: stfld System.Single WeaponVacuum::nextShot
IL_00bf: ldarg.0
IL_00c0: call System.Single WeaponVacuum::GetShootSpeedFactor()
IL_00c5: stloc.1
IL_00c6: ldarg.0
IL_00c7: ldfld UnityEngine.Animator WeaponVacuum::vacAnimator
IL_00cc: ldnull
IL_00cd: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_00d2: brfalse IL_00e3
IL_00d7: ldarg.0
IL_00d8: ldfld UnityEngine.Animator WeaponVacuum::vacAnimator
IL_00dd: ldloc.1
IL_00de: callvirt System.Void UnityEngine.Animator::set_speed(System.Single)
IL_00e3: ldarg.0
IL_00e4: ldfld System.Boolean WeaponVacuum::launchedHeld
IL_00e9: brtrue IL_015d
IL_00ee: ldarg.0
IL_00ef: ldfld WeaponVacuum/VacMode WeaponVacuum::vacMode
IL_00f4: ldc.i4.2
IL_00f5: bne.un IL_015d
IL_00fa: ldarg.0
IL_00fb: ldfld WeaponVacuum/VacAudioHandler WeaponVacuum::vacAudioHandler
IL_0100: ldc.i4.1
IL_0101: callvirt System.Void WeaponVacuum/VacAudioHandler::SetActive(System.Boolean)
IL_0106: ldarg.0
IL_0107: ldfld UnityEngine.GameObject WeaponVacuum::vacFX
IL_010c: ldarg.0
IL_010d: ldfld UnityEngine.GameObject WeaponVacuum::held
IL_0112: ldnull
IL_0113: call System.Boolean UnityEngine.Object::op_Equality(UnityEngine.Object,UnityEngine.Object)
IL_0118: callvirt System.Void UnityEngine.GameObject::SetActive(System.Boolean)
IL_011d: ldarg.0
IL_011e: ldfld SiloActivator WeaponVacuum::siloActivator
IL_0123: ldarg.0
IL_0124: ldfld UnityEngine.GameObject WeaponVacuum::held
IL_0129: ldnull
IL_012a: call System.Boolean UnityEngine.Object::op_Equality(UnityEngine.Object,UnityEngine.Object)
IL_012f: callvirt System.Void UnityEngine.Behaviour::set_enabled(System.Boolean)
IL_0134: ldarg.0
IL_0135: ldfld UnityEngine.GameObject WeaponVacuum::held
IL_013a: ldnull
IL_013b: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_0140: brfalse IL_0151
IL_0145: ldarg.0
IL_0146: ldloc.0
IL_0147: call System.Void WeaponVacuum::UpdateHeld(System.Collections.Generic.HashSet`1<UnityEngine.GameObject>)
IL_014c: br IL_0158
IL_0151: ldarg.0
IL_0152: ldloc.0
IL_0153: call System.Void WeaponVacuum::Consume(System.Collections.Generic.HashSet`1<UnityEngine.GameObject>)
IL_0158: br IL_0163
IL_015d: ldarg.0
IL_015e: call System.Void WeaponVacuum::ClearVac()
IL_0163: ldarg.0
IL_0164: call System.Void WeaponVacuum::UpdateVacAnimators()
IL_0169: ret

*/
//  GameData.Save
/*
IL_0000: ldarg.0
IL_0001: call System.Void GameData::ToSerializable()
IL_0006: ldarg.0
IL_0007: ldfld System.String GameData::gameName
IL_000c: brfalse IL_0022
IL_0011: ldarg.0
IL_0012: ldfld System.String GameData::gameName
IL_0017: callvirt System.Int32 System.String::get_Length()
IL_001c: ldc.i4.0
IL_001d: bgt IL_0028
IL_0022: newobj System.Void System.ArgumentException::.ctor()
IL_0027: throw
IL_0028: ldarg.0
IL_0029: ldfld System.String GameData::gameName
IL_002e: call System.String GameData::ToPath(System.String)
IL_0033: stloc.0
IL_0034: call System.Runtime.Serialization.Formatters.Binary.BinaryFormatter GameData::CreateFormatter()
IL_0039: stloc.1
IL_003a: ldloc.0
IL_003b: ldstr ".tmp"
IL_0040: call System.String System.String::Concat(System.String,System.String)
IL_0045: call System.IO.FileStream System.IO.File::Create(System.String)
IL_004a: stloc.2
IL_004b: ldloc.1
IL_004c: ldloc.2
IL_004d: ldc.i4.3
IL_004e: box System.Int32
IL_0053: callvirt System.Void System.Runtime.Serialization.Formatters.Binary.BinaryFormatter::Serialize(System.IO.Stream,System.Object)
IL_0058: ldarg.0
IL_0059: ldfld WorldData GameData::world
IL_005e: ldloc.1
IL_005f: ldloc.2
IL_0060: ldc.i4.6
IL_0061: callvirt System.Void DataModule`1<WorldData>::Serialize(System.Runtime.Serialization.Formatters.Binary.BinaryFormatter,System.IO.FileStream,System.Int32)
IL_0066: ldarg.0
IL_0067: ldfld PlayerData GameData::player
IL_006c: ldloc.1
IL_006d: ldloc.2
IL_006e: ldc.i4.3
IL_006f: callvirt System.Void DataModule`1<PlayerData>::Serialize(System.Runtime.Serialization.Formatters.Binary.BinaryFormatter,System.IO.FileStream,System.Int32)
IL_0074: ldarg.0
IL_0075: ldfld RanchData GameData::ranch
IL_007a: ldloc.1
IL_007b: ldloc.2
IL_007c: ldc.i4.3
IL_007d: callvirt System.Void DataModule`1<RanchData>::Serialize(System.Runtime.Serialization.Formatters.Binary.BinaryFormatter,System.IO.FileStream,System.Int32)
IL_0082: ldarg.0
IL_0083: ldfld ActorsData GameData::actors
IL_0088: ldloc.1
IL_0089: ldloc.2
IL_008a: ldc.i4.1
IL_008b: callvirt System.Void DataModule`1<ActorsData>::Serialize(System.Runtime.Serialization.Formatters.Binary.BinaryFormatter,System.IO.FileStream,System.Int32)
IL_0090: ldarg.0
IL_0091: ldfld PediaData GameData::pedia
IL_0096: ldloc.1
IL_0097: ldloc.2
IL_0098: ldc.i4.1
IL_0099: callvirt System.Void DataModule`1<PediaData>::Serialize(System.Runtime.Serialization.Formatters.Binary.BinaryFormatter,System.IO.FileStream,System.Int32)
IL_009e: ldarg.0
IL_009f: ldfld GameAchieveData GameData::achieve
IL_00a4: ldloc.1
IL_00a5: ldloc.2
IL_00a6: ldc.i4.1
IL_00a7: callvirt System.Void DataModule`1<GameAchieveData>::Serialize(System.Runtime.Serialization.Formatters.Binary.BinaryFormatter,System.IO.FileStream,System.Int32)
IL_00ac: leave IL_00be
IL_00b1: ldloc.2
IL_00b2: brfalse IL_00bd
IL_00b7: ldloc.2
IL_00b8: callvirt System.Void System.IDisposable::Dispose()
IL_00bd: endfinally
IL_00be: ldloc.0
IL_00bf: ldstr ".tmp"
IL_00c4: call System.String System.String::Concat(System.String,System.String)
IL_00c9: ldloc.0
IL_00ca: ldc.i4.1
IL_00cb: call System.Void System.IO.File::Copy(System.String,System.String,System.Boolean)
IL_00d0: ldloc.0
IL_00d1: ldstr ".tmp"
IL_00d6: call System.String System.String::Concat(System.String,System.String)
IL_00db: call System.Void System.IO.File::Delete(System.String)
IL_00e0: ret

*/
//  GameData.Load
/*
IL_0000: ldarg.0
IL_0001: call System.String GameData::ToPath(System.String)
IL_0006: stloc.0
IL_0007: ldarg.0
IL_0008: ldloc.0
IL_0009: ldarg.1
IL_000a: call GameData GameData::LoadPath(System.String,System.String,System.Boolean)
IL_000f: ret

*/
//  GameData.Load
/*
IL_0000: ldarg.0
IL_0001: call System.String GameData::ToPath(System.String)
IL_0006: stloc.0
IL_0007: ldarg.0
IL_0008: ldloc.0
IL_0009: ldarg.1
IL_000a: call GameData GameData::LoadPath(System.String,System.String,System.Boolean)
IL_000f: ret

*/
//  GameData.AvailableGames
/*
IL_0000: call System.String AutoSaveDirector::SavePath()
IL_0005: stloc.0
IL_0006: ldloc.0
IL_0007: call System.Boolean System.IO.Directory::Exists(System.String)
IL_000c: brtrue IL_0017
IL_0011: newobj System.Void System.Collections.Generic.List`1<GameData/Summary>::.ctor()
IL_0016: ret
IL_0017: ldloc.0
IL_0018: ldstr "*.sav"
IL_001d: call System.String[] System.IO.Directory::GetFiles(System.String,System.String)
IL_0022: stloc.1
IL_0023: newobj System.Void System.Collections.Generic.List`1<GameData/Summary>::.ctor()
IL_0028: stloc.2
IL_0029: ldloc.1
IL_002a: stloc.s V_4
IL_002c: ldc.i4.0
IL_002d: stloc.s V_5
IL_002f: br IL_0051
IL_0034: ldloc.s V_4
IL_0036: ldloc.s V_5
IL_0038: ldelem.ref
IL_0039: stloc.3
IL_003a: ldloc.2
IL_003b: ldloc.3
IL_003c: call System.String System.IO.Path::GetFileNameWithoutExtension(System.String)
IL_0041: call GameData/Summary GameData::LoadSummary(System.String)
IL_0046: callvirt System.Void System.Collections.Generic.List`1<GameData/Summary>::Add(!0)
IL_004b: ldloc.s V_5
IL_004d: ldc.i4.1
IL_004e: add
IL_004f: stloc.s V_5
IL_0051: ldloc.s V_5
IL_0053: ldloc.s V_4
IL_0055: ldlen
IL_0056: conv.i4
IL_0057: blt IL_0034
IL_005c: ldloc.2
IL_005d: ret

*/
//  GameData.ToPath
/*
IL_0000: call System.String AutoSaveDirector::SavePath()
IL_0005: ldarg.0
IL_0006: ldstr ".sav"
IL_000b: call System.String System.String::Concat(System.String,System.String)
IL_0010: call System.String System.IO.Path::Combine(System.String,System.String)
IL_0015: ret

*/
//  DirectedActorSpawner.Spawn
/*
IL_0000: newobj System.Void DirectedActorSpawner/<Spawn>c__Iterator1F::.ctor()
IL_0005: stloc.0
IL_0006: ldloc.0
IL_0007: ldarg.2
IL_0008: stfld Randoms DirectedActorSpawner/<Spawn>c__Iterator1F::rand
IL_000d: ldloc.0
IL_000e: ldarg.1
IL_000f: stfld System.Int32 DirectedActorSpawner/<Spawn>c__Iterator1F::count
IL_0014: ldloc.0
IL_0015: ldarg.2
IL_0016: stfld Randoms DirectedActorSpawner/<Spawn>c__Iterator1F::<$>rand
IL_001b: ldloc.0
IL_001c: ldarg.1
IL_001d: stfld System.Int32 DirectedActorSpawner/<Spawn>c__Iterator1F::<$>count
IL_0022: ldloc.0
IL_0023: ldarg.0
IL_0024: stfld DirectedActorSpawner DirectedActorSpawner/<Spawn>c__Iterator1F::<>f__this
IL_0029: ldloc.0
IL_002a: ret

*/
//  DirectedActorSpawner.Start
/*
IL_0000: ldarg.0
IL_0001: call T SRSingleton`1<SceneContext>::get_Instance()
IL_0006: callvirt TimeDirector SceneContext::get_TimeDirector()
IL_000b: stfld TimeDirector DirectedActorSpawner::timeDir
IL_0010: ldarg.0
IL_0011: ldarg.0
IL_0012: call !!0 UnityEngine.Component::GetComponentInParent<CellDirector>()
IL_0017: stfld CellDirector DirectedActorSpawner::cellDir
IL_001c: ldarg.0
IL_001d: ldarg.0
IL_001e: ldfld CellDirector DirectedActorSpawner::cellDir
IL_0023: callvirt System.Void DirectedActorSpawner::Register(CellDirector)
IL_0028: ret

*/
//  PlayerState.CanGetUpgrade
/*
IL_0000: ldarg.0
IL_0001: ldarg.1
IL_0002: call System.Boolean PlayerState::HasUpgrade(PlayerState/Upgrade)
IL_0007: brfalse IL_000e
IL_000c: ldc.i4.0
IL_000d: ret
IL_000e: ldarg.0
IL_000f: ldfld System.Collections.Generic.Dictionary`2<PlayerState/Upgrade,System.Single> PlayerState::upgradeLocks
IL_0014: ldarg.1
IL_0015: callvirt System.Boolean System.Collections.Generic.Dictionary`2<PlayerState/Upgrade,System.Single>::ContainsKey(!0)
IL_001a: brfalse IL_0021
IL_001f: ldc.i4.0
IL_0020: ret
IL_0021: ldarg.1
IL_0022: stloc.0
IL_0023: ldloc.0
IL_0024: ldc.i4.1
IL_0025: sub
IL_0026: switch IL_0058,IL_0060,IL_0091,IL_0068,IL_0070,IL_0091,IL_0078,IL_0080,IL_0091,IL_0088
IL_0053: br IL_0091
IL_0058: ldarg.0
IL_0059: ldc.i4.0
IL_005a: call System.Boolean PlayerState::HasUpgrade(PlayerState/Upgrade)
IL_005f: ret
IL_0060: ldarg.0
IL_0061: ldc.i4.1
IL_0062: call System.Boolean PlayerState::HasUpgrade(PlayerState/Upgrade)
IL_0067: ret
IL_0068: ldarg.0
IL_0069: ldc.i4.3
IL_006a: call System.Boolean PlayerState::HasUpgrade(PlayerState/Upgrade)
IL_006f: ret
IL_0070: ldarg.0
IL_0071: ldc.i4.4
IL_0072: call System.Boolean PlayerState::HasUpgrade(PlayerState/Upgrade)
IL_0077: ret
IL_0078: ldarg.0
IL_0079: ldc.i4.6
IL_007a: call System.Boolean PlayerState::HasUpgrade(PlayerState/Upgrade)
IL_007f: ret
IL_0080: ldarg.0
IL_0081: ldc.i4.7
IL_0082: call System.Boolean PlayerState::HasUpgrade(PlayerState/Upgrade)
IL_0087: ret
IL_0088: ldarg.0
IL_0089: ldc.i4.s 9
IL_008b: call System.Boolean PlayerState::HasUpgrade(PlayerState/Upgrade)
IL_0090: ret
IL_0091: ldc.i4.1
IL_0092: ret

*/
//  PlayerState.ApplyUpgrade
/*
IL_0000: call T SRSingleton`1<SceneContext>::get_Instance()
IL_0005: callvirt UnityEngine.GameObject SceneContext::get_Player()
IL_000a: callvirt !!0 UnityEngine.GameObject::GetComponent<EnergyJetpack>()
IL_000f: stloc.0
IL_0010: call T SRSingleton`1<SceneContext>::get_Instance()
IL_0015: callvirt UnityEngine.GameObject SceneContext::get_Player()
IL_001a: callvirt !!0 UnityEngine.GameObject::GetComponent<StaminaRun>()
IL_001f: stloc.1
IL_0020: call T SRSingleton`1<SceneContext>::get_Instance()
IL_0025: callvirt UnityEngine.GameObject SceneContext::get_Player()
IL_002a: callvirt !!0 UnityEngine.GameObject::GetComponentInChildren<WeaponVacuum>()
IL_002f: stloc.2
IL_0030: ldarg.1
IL_0031: stloc.3
IL_0032: ldloc.3
IL_0033: switch IL_00d0,IL_0146,IL_01bc,IL_0226,IL_0295,IL_0304,IL_0367,IL_039b,IL_03cf,IL_0075,IL_008e,IL_00c4,IL_00a9,IL_03f7
IL_0070: br IL_0408
IL_0075: ldloc.0
IL_0076: ldc.i4.1
IL_0077: callvirt System.Void EnergyJetpack::set_HasJetpack(System.Boolean)
IL_007c: ldarg.0
IL_007d: ldc.i4.s 10
IL_007f: ldc.r4 120
IL_0084: call System.Void PlayerState::MaybeAddUpgradeLock(PlayerState/Upgrade,System.Single)
IL_0089: br IL_0408
IL_008e: ldloc.0
IL_008f: ldloc.0
IL_0090: callvirt System.Single EnergyJetpack::get_Efficiency()
IL_0095: ldc.r4 0.8
IL_009a: call System.Single System.Math::Min(System.Single,System.Single)
IL_009f: callvirt System.Void EnergyJetpack::set_Efficiency(System.Single)
IL_00a4: br IL_0408
IL_00a9: ldloc.1
IL_00aa: ldloc.1
IL_00ab: callvirt System.Single StaminaRun::get_Efficiency()
IL_00b0: ldc.r4 0.8333
IL_00b5: call System.Single System.Math::Min(System.Single,System.Single)
IL_00ba: callvirt System.Void StaminaRun::set_Efficiency(System.Single)
IL_00bf: br IL_0408
IL_00c4: ldloc.2
IL_00c5: ldc.i4.1
IL_00c6: callvirt System.Void WeaponVacuum::set_HasAirBurst(System.Boolean)
IL_00cb: br IL_0408
IL_00d0: ldarg.0
IL_00d1: ldarg.0
IL_00d2: ldfld System.Int32 PlayerState::maxHealth
IL_00d7: ldarg.0
IL_00d8: ldfld System.Int32 PlayerState::defaultMaxHealth
IL_00dd: conv.r4
IL_00de: ldc.r4 1.5
IL_00e3: mul
IL_00e4: call System.Int32 UnityEngine.Mathf::RoundToInt(System.Single)
IL_00e9: call System.Int32 System.Math::Max(System.Int32,System.Int32)
IL_00ee: stfld System.Int32 PlayerState::maxHealth
IL_00f3: ldarg.0
IL_00f4: ldfld System.Single PlayerState::currHealth
IL_00f9: ldarg.0
IL_00fa: ldfld System.Int32 PlayerState::maxHealth
IL_00ff: conv.r4
IL_0100: bge.un IL_0135
IL_0105: ldarg.0
IL_0106: ldarg.0
IL_0107: ldfld System.Single PlayerState::healthBurstAfter
IL_010c: ldarg.0
IL_010d: ldfld TimeDirector PlayerState::timeDir
IL_0112: callvirt System.Single TimeDirector::WorldTime()
IL_0117: ldc.r4 60
IL_011c: ldarg.0
IL_011d: ldfld System.Single PlayerState::healthBurstAmount
IL_0122: mul
IL_0123: ldarg.0
IL_0124: ldfld System.Single PlayerState::healthRecoveredPerSecond
IL_0129: div
IL_012a: add
IL_012b: call System.Single System.Math::Min(System.Single,System.Single)
IL_0130: stfld System.Single PlayerState::healthBurstAfter
IL_0135: ldarg.0
IL_0136: ldc.i4.1
IL_0137: ldc.r4 48
IL_013c: call System.Void PlayerState::MaybeAddUpgradeLock(PlayerState/Upgrade,System.Single)
IL_0141: br IL_0408
IL_0146: ldarg.0
IL_0147: ldarg.0
IL_0148: ldfld System.Int32 PlayerState::maxHealth
IL_014d: ldarg.0
IL_014e: ldfld System.Int32 PlayerState::defaultMaxHealth
IL_0153: conv.r4
IL_0154: ldc.r4 2
IL_0159: mul
IL_015a: call System.Int32 UnityEngine.Mathf::RoundToInt(System.Single)
IL_015f: call System.Int32 System.Math::Max(System.Int32,System.Int32)
IL_0164: stfld System.Int32 PlayerState::maxHealth
IL_0169: ldarg.0
IL_016a: ldfld System.Single PlayerState::currHealth
IL_016f: ldarg.0
IL_0170: ldfld System.Int32 PlayerState::maxHealth
IL_0175: conv.r4
IL_0176: bge.un IL_01ab
IL_017b: ldarg.0
IL_017c: ldarg.0
IL_017d: ldfld System.Single PlayerState::healthBurstAfter
IL_0182: ldarg.0
IL_0183: ldfld TimeDirector PlayerState::timeDir
IL_0188: callvirt System.Single TimeDirector::WorldTime()
IL_018d: ldc.r4 60
IL_0192: ldarg.0
IL_0193: ldfld System.Single PlayerState::healthBurstAmount
IL_0198: mul
IL_0199: ldarg.0
IL_019a: ldfld System.Single PlayerState::healthRecoveredPerSecond
IL_019f: div
IL_01a0: add
IL_01a1: call System.Single System.Math::Min(System.Single,System.Single)
IL_01a6: stfld System.Single PlayerState::healthBurstAfter
IL_01ab: ldarg.0
IL_01ac: ldc.i4.2
IL_01ad: ldc.r4 72
IL_01b2: call System.Void PlayerState::MaybeAddUpgradeLock(PlayerState/Upgrade,System.Single)
IL_01b7: br IL_0408
IL_01bc: ldarg.0
IL_01bd: ldarg.0
IL_01be: ldfld System.Int32 PlayerState::maxHealth
IL_01c3: ldarg.0
IL_01c4: ldfld System.Int32 PlayerState::defaultMaxHealth
IL_01c9: conv.r4
IL_01ca: ldc.r4 2.5
IL_01cf: mul
IL_01d0: call System.Int32 UnityEngine.Mathf::RoundToInt(System.Single)
IL_01d5: call System.Int32 System.Math::Max(System.Int32,System.Int32)
IL_01da: stfld System.Int32 PlayerState::maxHealth
IL_01df: ldarg.0
IL_01e0: ldfld System.Single PlayerState::currHealth
IL_01e5: ldarg.0
IL_01e6: ldfld System.Int32 PlayerState::maxHealth
IL_01eb: conv.r4
IL_01ec: bge.un IL_0221
IL_01f1: ldarg.0
IL_01f2: ldarg.0
IL_01f3: ldfld System.Single PlayerState::healthBurstAfter
IL_01f8: ldarg.0
IL_01f9: ldfld TimeDirector PlayerState::timeDir
IL_01fe: callvirt System.Single TimeDirector::WorldTime()
IL_0203: ldc.r4 60
IL_0208: ldarg.0
IL_0209: ldfld System.Single PlayerState::healthBurstAmount
IL_020e: mul
IL_020f: ldarg.0
IL_0210: ldfld System.Single PlayerState::healthRecoveredPerSecond
IL_0215: div
IL_0216: add
IL_0217: call System.Single System.Math::Min(System.Single,System.Single)
IL_021c: stfld System.Single PlayerState::healthBurstAfter
IL_0221: br IL_0408
IL_0226: ldarg.0
IL_0227: ldarg.0
IL_0228: ldfld System.Int32 PlayerState::maxEnergy
IL_022d: ldarg.0
IL_022e: ldfld System.Int32 PlayerState::defaultMaxEnergy
IL_0233: conv.r4
IL_0234: ldc.r4 1.5
IL_0239: mul
IL_023a: call System.Int32 UnityEngine.Mathf::RoundToInt(System.Single)
IL_023f: call System.Int32 System.Math::Max(System.Int32,System.Int32)
IL_0244: stfld System.Int32 PlayerState::maxEnergy
IL_0249: ldarg.0
IL_024a: ldfld System.Single PlayerState::currEnergy
IL_024f: ldarg.0
IL_0250: ldfld System.Int32 PlayerState::maxEnergy
IL_0255: conv.r4
IL_0256: bge.un IL_0284
IL_025b: ldarg.0
IL_025c: ldarg.0
IL_025d: ldfld System.Single PlayerState::energyRecoverAfter
IL_0262: ldarg.0
IL_0263: ldfld TimeDirector PlayerState::timeDir
IL_0268: callvirt System.Single TimeDirector::WorldTime()
IL_026d: ldc.r4 60
IL_0272: ldarg.0
IL_0273: ldfld System.Single PlayerState::energyRecoveryDelay
IL_0278: mul
IL_0279: add
IL_027a: call System.Single System.Math::Min(System.Single,System.Single)
IL_027f: stfld System.Single PlayerState::energyRecoverAfter
IL_0284: ldarg.0
IL_0285: ldc.i4.4
IL_0286: ldc.r4 48
IL_028b: call System.Void PlayerState::MaybeAddUpgradeLock(PlayerState/Upgrade,System.Single)
IL_0290: br IL_0408
IL_0295: ldarg.0
IL_0296: ldarg.0
IL_0297: ldfld System.Int32 PlayerState::maxEnergy
IL_029c: ldarg.0
IL_029d: ldfld System.Int32 PlayerState::defaultMaxEnergy
IL_02a2: conv.r4
IL_02a3: ldc.r4 2
IL_02a8: mul
IL_02a9: call System.Int32 UnityEngine.Mathf::RoundToInt(System.Single)
IL_02ae: call System.Int32 System.Math::Max(System.Int32,System.Int32)
IL_02b3: stfld System.Int32 PlayerState::maxEnergy
IL_02b8: ldarg.0
IL_02b9: ldfld System.Single PlayerState::currEnergy
IL_02be: ldarg.0
IL_02bf: ldfld System.Int32 PlayerState::maxEnergy
IL_02c4: conv.r4
IL_02c5: bge.un IL_02f3
IL_02ca: ldarg.0
IL_02cb: ldarg.0
IL_02cc: ldfld System.Single PlayerState::energyRecoverAfter
IL_02d1: ldarg.0
IL_02d2: ldfld TimeDirector PlayerState::timeDir
IL_02d7: callvirt System.Single TimeDirector::WorldTime()
IL_02dc: ldc.r4 60
IL_02e1: ldarg.0
IL_02e2: ldfld System.Single PlayerState::energyRecoveryDelay
IL_02e7: mul
IL_02e8: add
IL_02e9: call System.Single System.Math::Min(System.Single,System.Single)
IL_02ee: stfld System.Single PlayerState::energyRecoverAfter
IL_02f3: ldarg.0
IL_02f4: ldc.i4.5
IL_02f5: ldc.r4 72
IL_02fa: call System.Void PlayerState::MaybeAddUpgradeLock(PlayerState/Upgrade,System.Single)
IL_02ff: br IL_0408
IL_0304: ldarg.0
IL_0305: ldarg.0
IL_0306: ldfld System.Int32 PlayerState::maxEnergy
IL_030b: ldarg.0
IL_030c: ldfld System.Int32 PlayerState::defaultMaxEnergy
IL_0311: conv.r4
IL_0312: ldc.r4 2.5
IL_0317: mul
IL_0318: call System.Int32 UnityEngine.Mathf::RoundToInt(System.Single)
IL_031d: call System.Int32 System.Math::Max(System.Int32,System.Int32)
IL_0322: stfld System.Int32 PlayerState::maxEnergy
IL_0327: ldarg.0
IL_0328: ldfld System.Single PlayerState::currEnergy
IL_032d: ldarg.0
IL_032e: ldfld System.Int32 PlayerState::maxEnergy
IL_0333: conv.r4
IL_0334: bge.un IL_0362
IL_0339: ldarg.0
IL_033a: ldarg.0
IL_033b: ldfld System.Single PlayerState::energyRecoverAfter
IL_0340: ldarg.0
IL_0341: ldfld TimeDirector PlayerState::timeDir
IL_0346: callvirt System.Single TimeDirector::WorldTime()
IL_034b: ldc.r4 60
IL_0350: ldarg.0
IL_0351: ldfld System.Single PlayerState::energyRecoveryDelay
IL_0356: mul
IL_0357: add
IL_0358: call System.Single System.Math::Min(System.Single,System.Single)
IL_035d: stfld System.Single PlayerState::energyRecoverAfter
IL_0362: br IL_0408
IL_0367: ldarg.0
IL_0368: ldarg.0
IL_0369: ldfld System.Int32 PlayerState::maxAmmo
IL_036e: ldarg.0
IL_036f: ldfld System.Int32 PlayerState::defaultMaxAmmo
IL_0374: conv.r4
IL_0375: ldc.r4 1.5
IL_037a: mul
IL_037b: call System.Int32 UnityEngine.Mathf::RoundToInt(System.Single)
IL_0380: call System.Int32 System.Math::Max(System.Int32,System.Int32)
IL_0385: stfld System.Int32 PlayerState::maxAmmo
IL_038a: ldarg.0
IL_038b: ldc.i4.7
IL_038c: ldc.r4 48
IL_0391: call System.Void PlayerState::MaybeAddUpgradeLock(PlayerState/Upgrade,System.Single)
IL_0396: br IL_0408
IL_039b: ldarg.0
IL_039c: ldarg.0
IL_039d: ldfld System.Int32 PlayerState::maxAmmo
IL_03a2: ldarg.0
IL_03a3: ldfld System.Int32 PlayerState::defaultMaxAmmo
IL_03a8: conv.r4
IL_03a9: ldc.r4 2
IL_03ae: mul
IL_03af: call System.Int32 UnityEngine.Mathf::RoundToInt(System.Single)
IL_03b4: call System.Int32 System.Math::Max(System.Int32,System.Int32)
IL_03b9: stfld System.Int32 PlayerState::maxAmmo
IL_03be: ldarg.0
IL_03bf: ldc.i4.8
IL_03c0: ldc.r4 72
IL_03c5: call System.Void PlayerState::MaybeAddUpgradeLock(PlayerState/Upgrade,System.Single)
IL_03ca: br IL_0408
IL_03cf: ldarg.0
IL_03d0: ldarg.0
IL_03d1: ldfld System.Int32 PlayerState::maxAmmo
IL_03d6: ldarg.0
IL_03d7: ldfld System.Int32 PlayerState::defaultMaxAmmo
IL_03dc: conv.r4
IL_03dd: ldc.r4 2.5
IL_03e2: mul
IL_03e3: call System.Int32 UnityEngine.Mathf::RoundToInt(System.Single)
IL_03e8: call System.Int32 System.Math::Max(System.Int32,System.Int32)
IL_03ed: stfld System.Int32 PlayerState::maxAmmo
IL_03f2: br IL_0408
IL_03f7: ldarg.0
IL_03f8: ldfld Ammo PlayerState::ammo
IL_03fd: ldc.i4.5
IL_03fe: callvirt System.Void Ammo::IncreaseUsableSlots(System.Int32)
IL_0403: br IL_0408
IL_0408: ret

*/
//  PlayerState.Damage
/*
IL_0000: ldarg.0
IL_0001: dup
IL_0002: ldfld System.Single PlayerState::currHealth
IL_0007: ldarg.1
IL_0008: conv.r4
IL_0009: sub
IL_000a: stfld System.Single PlayerState::currHealth
IL_000f: ldarg.0
IL_0010: ldfld System.Single PlayerState::currHealth
IL_0015: ldc.r4 0
IL_001a: bgt.un IL_0037
IL_001f: ldarg.0
IL_0020: ldc.r4 0
IL_0025: stfld System.Single PlayerState::currHealth
IL_002a: ldarg.0
IL_002b: ldc.r4 Infinity
IL_0030: stfld System.Single PlayerState::healthBurstAfter
IL_0035: ldc.i4.1
IL_0036: ret
IL_0037: ldarg.0
IL_0038: ldarg.0
IL_0039: ldfld TimeDirector PlayerState::timeDir
IL_003e: callvirt System.Single TimeDirector::WorldTime()
IL_0043: ldc.r4 60
IL_0048: ldarg.0
IL_0049: ldfld System.Single PlayerState::healthBurstAmount
IL_004e: mul
IL_004f: ldarg.0
IL_0050: ldfld System.Single PlayerState::healthRecoveredPerSecond
IL_0055: div
IL_0056: add
IL_0057: stfld System.Single PlayerState::healthBurstAfter
IL_005c: ldc.i4.0
IL_005d: ret

*/
//  PlayerState.SpendEnergy
/*
IL_0000: ldarg.0
IL_0001: ldc.r4 0
IL_0006: ldarg.0
IL_0007: ldfld System.Single PlayerState::currEnergy
IL_000c: ldarg.1
IL_000d: sub
IL_000e: call System.Single UnityEngine.Mathf::Max(System.Single,System.Single)
IL_0013: stfld System.Single PlayerState::currEnergy
IL_0018: ldarg.0
IL_0019: ldarg.0
IL_001a: ldfld TimeDirector PlayerState::timeDir
IL_001f: callvirt System.Single TimeDirector::WorldTime()
IL_0024: ldc.r4 60
IL_0029: ldarg.0
IL_002a: ldfld System.Single PlayerState::energyRecoveryDelay
IL_002f: mul
IL_0030: add
IL_0031: stfld System.Single PlayerState::energyRecoverAfter
IL_0036: ret

*/
//  PlayerState.SetEnergy
/*
IL_0000: ldarg.0
IL_0001: ldarg.1
IL_0002: conv.r4
IL_0003: stfld System.Single PlayerState::currEnergy
IL_0008: ldarg.0
IL_0009: ldfld System.Single PlayerState::currEnergy
IL_000e: ldarg.0
IL_000f: ldfld System.Int32 PlayerState::maxEnergy
IL_0014: conv.r4
IL_0015: bge.un IL_0043
IL_001a: ldarg.0
IL_001b: ldarg.0
IL_001c: ldfld System.Single PlayerState::energyRecoverAfter
IL_0021: ldarg.0
IL_0022: ldfld TimeDirector PlayerState::timeDir
IL_0027: callvirt System.Single TimeDirector::WorldTime()
IL_002c: ldc.r4 60
IL_0031: ldarg.0
IL_0032: ldfld System.Single PlayerState::energyRecoveryDelay
IL_0037: mul
IL_0038: add
IL_0039: call System.Single System.Math::Min(System.Single,System.Single)
IL_003e: stfld System.Single PlayerState::energyRecoverAfter
IL_0043: ret

*/
//  PlayerState.AddCurrency
/*
IL_0000: ldarg.0
IL_0001: dup
IL_0002: ldfld System.Int32 PlayerState::currency
IL_0007: ldarg.1
IL_0008: add
IL_0009: stfld System.Int32 PlayerState::currency
IL_000e: ldarg.0
IL_000f: dup
IL_0010: ldfld System.Int32 PlayerState::currencyEverCollected
IL_0015: ldarg.1
IL_0016: add
IL_0017: stfld System.Int32 PlayerState::currencyEverCollected
IL_001c: ldarg.0
IL_001d: ldfld System.Int32 PlayerState::currencyEverCollected
IL_0022: ldarg.0
IL_0023: ldfld System.Int32 PlayerState::currencyPerProgress
IL_0028: div
IL_0029: ldarg.0
IL_002a: ldfld System.Int32 PlayerState::currencyEverCollected
IL_002f: ldarg.1
IL_0030: sub
IL_0031: ldarg.0
IL_0032: ldfld System.Int32 PlayerState::currencyPerProgress
IL_0037: div
IL_0038: sub
IL_0039: stloc.0
IL_003a: ldloc.0
IL_003b: ldc.i4.0
IL_003c: ble IL_006a
IL_0041: call T SRSingleton`1<SceneContext>::get_Instance()
IL_0046: callvirt ProgressDirector SceneContext::get_ProgressDirector()
IL_004b: stloc.1
IL_004c: ldc.i4.0
IL_004d: stloc.2
IL_004e: br IL_0063
IL_0053: ldloc.1
IL_0054: ldarg.0
IL_0055: ldfld ProgressDirector/ProgressType PlayerState::currencyProgressType
IL_005a: callvirt System.Void ProgressDirector::AddProgress(ProgressDirector/ProgressType)
IL_005f: ldloc.2
IL_0060: ldc.i4.1
IL_0061: add
IL_0062: stloc.2
IL_0063: ldloc.2
IL_0064: ldloc.0
IL_0065: blt IL_0053
IL_006a: ldarg.1
IL_006b: ldc.i4.0
IL_006c: ble IL_008b
IL_0071: ldarg.0
IL_0072: ldfld AchievementsDirector PlayerState::achieveDir
IL_0077: ldc.i4.2
IL_0078: ldarg.1
IL_0079: callvirt System.Void AchievementsDirector::AddToStat(AchievementsDirector/IntStat,System.Int32)
IL_007e: ldarg.0
IL_007f: ldfld AchievementsDirector PlayerState::achieveDir
IL_0084: ldc.i4.3
IL_0085: ldarg.1
IL_0086: callvirt System.Void AchievementsDirector::AddToStat(AchievementsDirector/IntStat,System.Int32)
IL_008b: ret

*/
//  PlayerState.AddRads
/*
IL_0000: ldarg.0
IL_0001: dup
IL_0002: ldfld System.Single PlayerState::currRads
IL_0007: ldarg.1
IL_0008: add
IL_0009: stfld System.Single PlayerState::currRads
IL_000e: ldarg.0
IL_000f: ldarg.0
IL_0010: ldfld TimeDirector PlayerState::timeDir
IL_0015: callvirt System.Single TimeDirector::WorldTime()
IL_001a: ldc.r4 60
IL_001f: ldarg.0
IL_0020: ldfld System.Single PlayerState::currRads
IL_0025: ldarg.0
IL_0026: ldfld System.Int32 PlayerState::maxRads
IL_002b: conv.r4
IL_002c: blt.un IL_003c
IL_0031: ldarg.0
IL_0032: ldfld System.Single PlayerState::fullRadRecoveryDelay
IL_0037: br IL_0042
IL_003c: ldarg.0
IL_003d: ldfld System.Single PlayerState::nonfullRadRecoveryDelay
IL_0042: mul
IL_0043: add
IL_0044: stfld System.Single PlayerState::radRecoverAfter
IL_0049: ldarg.0
IL_004a: ldfld System.Single PlayerState::currRads
IL_004f: ldarg.0
IL_0050: ldfld System.Int32 PlayerState::maxRads
IL_0055: conv.r4
IL_0056: ble.un IL_0098
IL_005b: ldarg.0
IL_005c: ldfld System.Single PlayerState::currRads
IL_0061: ldarg.0
IL_0062: ldfld System.Int32 PlayerState::maxRads
IL_0067: conv.r4
IL_0068: sub
IL_0069: ldarg.0
IL_006a: ldfld System.Int32 PlayerState::radUnitDamage
IL_006f: conv.r4
IL_0070: div
IL_0071: call System.Int32 UnityEngine.Mathf::FloorToInt(System.Single)
IL_0076: stloc.0
IL_0077: ldloc.0
IL_0078: ldc.i4.0
IL_0079: ble IL_0098
IL_007e: ldarg.0
IL_007f: ldfld System.Int32 PlayerState::radUnitDamage
IL_0084: ldloc.0
IL_0085: mul
IL_0086: stloc.1
IL_0087: ldarg.0
IL_0088: dup
IL_0089: ldfld System.Single PlayerState::currRads
IL_008e: ldloc.1
IL_008f: conv.r4
IL_0090: sub
IL_0091: stfld System.Single PlayerState::currRads
IL_0096: ldloc.1
IL_0097: ret
IL_0098: ldc.i4.0
IL_0099: ret

*/
//  PlayerDeathHandler.OnDeath
/*
IL_0000: ldarg.0
IL_0001: ldfld System.Boolean PlayerDeathHandler::deathInProgress
IL_0006: brfalse IL_000c
IL_000b: ret
IL_000c: ldarg.0
IL_000d: ldc.i4.1
IL_000e: stfld System.Boolean PlayerDeathHandler::deathInProgress
IL_0013: call T SRSingleton`1<SceneContext>::get_Instance()
IL_0018: callvirt TimeDirector SceneContext::get_TimeDirector()
IL_001d: stloc.0
IL_001e: call T SRSingleton`1<SceneContext>::get_Instance()
IL_0023: callvirt PlayerState SceneContext::get_PlayerState()
IL_0028: stloc.1
IL_0029: ldloc.1
IL_002a: callvirt GameModeSettings PlayerState::get_ModeSettings()
IL_002f: ldfld System.Boolean GameModeSettings::hoursTilDawnOnDeath
IL_0034: brfalse IL_0044
IL_0039: ldloc.0
IL_003a: callvirt System.Single TimeDirector::GetNextDawnAfterNextDusk()
IL_003f: br IL_0055
IL_0044: ldloc.0
IL_0045: ldloc.1
IL_0046: callvirt GameModeSettings PlayerState::get_ModeSettings()
IL_004b: ldfld System.Single GameModeSettings::hoursLostOnDeath
IL_0050: callvirt System.Single TimeDirector::HoursFromNow(System.Single)
IL_0055: stloc.2
IL_0056: ldloc.1
IL_0057: callvirt GameModeSettings PlayerState::get_ModeSettings()
IL_005c: ldfld System.Boolean GameModeSettings::hoursTilDawnOnDeath
IL_0061: brfalse IL_009d
IL_0066: call T SRSingleton`1<SceneContext>::get_Instance()
IL_006b: callvirt TimeDirector SceneContext::get_TimeDirector()
IL_0070: callvirt System.Single TimeDirector::CurrHour()
IL_0075: stloc.3
IL_0076: ldloc.3
IL_0077: ldc.r4 10
IL_007c: bge.un IL_009d
IL_0081: ldloc.3
IL_0082: ldc.r4 6
IL_0087: blt.un IL_009d
IL_008c: call T SRSingleton`1<SceneContext>::get_Instance()
IL_0091: callvirt AchievementsDirector SceneContext::get_AchievementsDirector()
IL_0096: ldc.i4.4
IL_0097: ldc.i4.1
IL_0098: callvirt System.Void AchievementsDirector::AddToStat(AchievementsDirector/IntStat,System.Int32)
IL_009d: call T SRSingleton`1<SceneContext>::get_Instance()
IL_00a2: callvirt AchievementsDirector SceneContext::get_AchievementsDirector()
IL_00a7: ldc.i4.0
IL_00a8: ldc.i4.1
IL_00a9: callvirt System.Void AchievementsDirector::AddToStat(AchievementsDirector/GameIntStat,System.Int32)
IL_00ae: ldloc.1
IL_00af: callvirt GameModeSettings PlayerState::get_ModeSettings()
IL_00b4: ldfld System.Single GameModeSettings::pctCurrencyLostOnDeath
IL_00b9: ldc.r4 0
IL_00be: ble.un IL_00e2
IL_00c3: ldloc.1
IL_00c4: ldloc.1
IL_00c5: callvirt System.Int32 PlayerState::GetCurrency()
IL_00ca: conv.r4
IL_00cb: ldloc.1
IL_00cc: callvirt GameModeSettings PlayerState::get_ModeSettings()
IL_00d1: ldfld System.Single GameModeSettings::pctCurrencyLostOnDeath
IL_00d6: mul
IL_00d7: call System.Int32 UnityEngine.Mathf::FloorToInt(System.Single)
IL_00dc: ldc.i4.1
IL_00dd: callvirt System.Void PlayerState::SpendCurrency(System.Int32,System.Boolean)
IL_00e2: ldarg.0
IL_00e3: call !!0 UnityEngine.Component::GetComponent<LockOnDeath>()
IL_00e8: ldloc.2
IL_00e9: ldc.r4 5
IL_00ee: ldarg.0
IL_00ef: ldftn System.Void PlayerDeathHandler::<OnDeath>m__22()
IL_00f5: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_00fa: callvirt System.Void LockOnDeath::LockUntil(System.Single,System.Single,UnityEngine.Events.UnityAction)
IL_00ff: ldstr "PlayerDeath"
IL_0104: ldnull
IL_0105: call UnityEngine.Analytics.AnalyticsResult UnityEngine.Analytics.Analytics::CustomEvent(System.String,System.Collections.Generic.IDictionary`2<System.String,System.Object>)
IL_010a: pop
IL_010b: ldarg.0
IL_010c: ldarg.0
IL_010d: ldc.i4.1
IL_010e: ldc.r4 1
IL_0113: ldnull
IL_0114: call System.Collections.IEnumerator PlayerDeathHandler::ResetPlayer(System.Boolean,System.Single,UnityEngine.Events.UnityAction)
IL_0119: call UnityEngine.Coroutine UnityEngine.MonoBehaviour::StartCoroutine(System.Collections.IEnumerator)
IL_011e: pop
IL_011f: ldarg.0
IL_0120: ldarg.0
IL_0121: call System.Collections.IEnumerator PlayerDeathHandler::DisplayDeathUI()
IL_0126: call UnityEngine.Coroutine UnityEngine.MonoBehaviour::StartCoroutine(System.Collections.IEnumerator)
IL_012b: pop
IL_012c: ret

*/
//  LockOnDeath.LockUntil
/*
IL_0000: ldarg.0
IL_0001: ldfld AchievementsDirector LockOnDeath::achieveDir
IL_0006: ldc.i4.2
IL_0007: ldarg.0
IL_0008: ldfld TimeDirector LockOnDeath::timeDir
IL_000d: callvirt System.Single TimeDirector::WorldTime()
IL_0012: callvirt System.Void AchievementsDirector::SetStat(AchievementsDirector/GameFloatStat,System.Single)
IL_0017: ldarg.0
IL_0018: ldarg.1
IL_0019: stfld System.Single LockOnDeath::unlockWorldTime
IL_001e: ldarg.0
IL_001f: ldc.i4.1
IL_0020: stfld System.Boolean LockOnDeath::locked
IL_0025: ldarg.0
IL_0026: call System.Void LockOnDeath::Freeze()
IL_002b: ldarg.0
IL_002c: ldfld LockOnDeath/OnLockChanged LockOnDeath::onLockChanged
IL_0031: brfalse IL_0042
IL_0036: ldarg.0
IL_0037: ldfld LockOnDeath/OnLockChanged LockOnDeath::onLockChanged
IL_003c: ldc.i4.1
IL_003d: callvirt System.Void LockOnDeath/OnLockChanged::Invoke(System.Boolean)
IL_0042: ldarg.0
IL_0043: ldarg.3
IL_0044: stfld UnityEngine.Events.UnityAction LockOnDeath::onLockComplete
IL_0049: ldarg.0
IL_004a: ldarg.0
IL_004b: ldarg.2
IL_004c: call System.Collections.IEnumerator LockOnDeath::DelayedFastForward(System.Single)
IL_0051: call UnityEngine.Coroutine UnityEngine.MonoBehaviour::StartCoroutine(System.Collections.IEnumerator)
IL_0056: pop
IL_0057: ret

*/
//  LockOnDeath.Update
/*
IL_0000: ldarg.0
IL_0001: ldfld System.Boolean LockOnDeath::locked
IL_0006: brfalse IL_00c3
IL_000b: ldarg.0
IL_000c: ldfld TimeDirector LockOnDeath::timeDir
IL_0011: ldarg.0
IL_0012: ldfld System.Single LockOnDeath::unlockWorldTime
IL_0017: callvirt System.Boolean TimeDirector::HasReached(System.Single)
IL_001c: brfalse IL_00c3
IL_0021: ldarg.0
IL_0022: ldc.i4.0
IL_0023: stfld System.Boolean LockOnDeath::locked
IL_0028: ldarg.0
IL_0029: call System.Void LockOnDeath::Unfreeze()
IL_002e: ldarg.0
IL_002f: ldfld LockOnDeath/OnLockChanged LockOnDeath::onLockChanged
IL_0034: brfalse IL_0045
IL_0039: ldarg.0
IL_003a: ldfld LockOnDeath/OnLockChanged LockOnDeath::onLockChanged
IL_003f: ldc.i4.0
IL_0040: callvirt System.Void LockOnDeath/OnLockChanged::Invoke(System.Boolean)
IL_0045: ldarg.0
IL_0046: ldfld TimeDirector LockOnDeath::timeDir
IL_004b: ldc.i4.0
IL_004c: callvirt System.Void TimeDirector::SetFastForward(System.Boolean)
IL_0051: ldarg.0
IL_0052: ldflda SECTR_AudioCueInstance LockOnDeath::timePassingInstance
IL_0057: ldc.i4.0
IL_0058: call System.Void SECTR_AudioCueInstance::Stop(System.Boolean)
IL_005d: ldarg.0
IL_005e: ldfld SECTR_AudioCue LockOnDeath::playerWakeCue
IL_0063: ldnull
IL_0064: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_0069: brfalse IL_0087
IL_006e: ldarg.0
IL_006f: call !!0 UnityEngine.Component::GetComponent<SECTR_AudioSource>()
IL_0074: stloc.0
IL_0075: ldloc.0
IL_0076: ldarg.0
IL_0077: ldfld SECTR_AudioCue LockOnDeath::playerWakeCue
IL_007c: stfld SECTR_AudioCue SECTR_AudioSource::Cue
IL_0081: ldloc.0
IL_0082: callvirt System.Void SECTR_AudioSource::Play()
IL_0087: call T SRSingleton`1<GameContext>::get_Instance()
IL_008c: callvirt AutoSaveDirector GameContext::get_AutoSaveDirector()
IL_0091: callvirt System.Void AutoSaveDirector::SaveGame()
IL_0096: ldarg.0
IL_0097: ldfld AchievementsDirector LockOnDeath::achieveDir
IL_009c: ldc.i4.3
IL_009d: ldarg.0
IL_009e: ldfld TimeDirector LockOnDeath::timeDir
IL_00a3: callvirt System.Single TimeDirector::WorldTime()
IL_00a8: callvirt System.Void AchievementsDirector::SetStat(AchievementsDirector/GameFloatStat,System.Single)
IL_00ad: ldarg.0
IL_00ae: ldfld UnityEngine.Events.UnityAction LockOnDeath::onLockComplete
IL_00b3: brfalse IL_00c3
IL_00b8: ldarg.0
IL_00b9: ldfld UnityEngine.Events.UnityAction LockOnDeath::onLockComplete
IL_00be: callvirt System.Void UnityEngine.Events.UnityAction::Invoke()
IL_00c3: ret

*/
//  CellDirector.Update
/*
IL_0000: ldarg.0
IL_0001: ldfld System.Boolean CellDirector::allowSpawns
IL_0006: brtrue IL_000c
IL_000b: ret
IL_000c: call System.Single UnityEngine.Time::get_time()
IL_0011: ldarg.0
IL_0012: ldfld System.Single CellDirector::spawnThrottleTime
IL_0017: blt.un IL_030c
IL_001c: ldarg.0
IL_001d: ldfld TimeDirector CellDirector::timeDir
IL_0022: ldarg.0
IL_0023: ldfld System.Single CellDirector::nextSpawn
IL_0028: callvirt System.Boolean TimeDirector::HasReached(System.Single)
IL_002d: brfalse IL_0293
IL_0032: ldarg.0
IL_0033: ldfld SECTR_Sector CellDirector::sector
IL_0038: callvirt System.Boolean SECTR_Member::get_Hibernate()
IL_003d: brtrue IL_0293
IL_0042: ldarg.0
IL_0043: ldfld System.Collections.Generic.List`1<DirectedSlimeSpawner> CellDirector::spawners
IL_0048: callvirt System.Int32 System.Collections.Generic.List`1<DirectedSlimeSpawner>::get_Count()
IL_004d: ldc.i4.0
IL_004e: ble IL_0148
IL_0053: ldarg.0
IL_0054: call System.Boolean CellDirector::NeedsMoreSlimes()
IL_0059: brfalse IL_0148
IL_005e: ldarg.0
IL_005f: call System.Boolean CellDirector::HasTarrSlimes()
IL_0064: brtrue IL_0148
IL_0069: newobj System.Void System.Collections.Generic.Dictionary`2<DirectedSlimeSpawner,System.Single>::.ctor()
IL_006e: stloc.0
IL_006f: ldc.r4 0
IL_0074: stloc.1
IL_0075: ldarg.0
IL_0076: ldfld System.Collections.Generic.List`1<DirectedSlimeSpawner> CellDirector::spawners
IL_007b: callvirt System.Collections.Generic.List`1/Enumerator<!0> System.Collections.Generic.List`1<DirectedSlimeSpawner>::GetEnumerator()
IL_0080: stloc.3
IL_0081: br IL_00b9
IL_0086: ldloca.s V_3
IL_0088: call !0 System.Collections.Generic.List`1/Enumerator<DirectedSlimeSpawner>::get_Current()
IL_008d: stloc.2
IL_008e: ldloc.2
IL_008f: ldloca.s V_10
IL_0091: initobj System.Nullable`1<System.Single>
IL_0097: ldloc.s V_10
IL_0099: callvirt System.Boolean DirectedActorSpawner::CanSpawn(System.Nullable`1<System.Single>)
IL_009e: brfalse IL_00b9
IL_00a3: ldloc.0
IL_00a4: ldloc.2
IL_00a5: ldloc.2
IL_00a6: ldfld System.Single DirectedActorSpawner::directedSpawnWeight
IL_00ab: callvirt System.Void System.Collections.Generic.Dictionary`2<DirectedSlimeSpawner,System.Single>::set_Item(!0,!1)
IL_00b0: ldloc.1
IL_00b1: ldloc.2
IL_00b2: ldfld System.Single DirectedActorSpawner::directedSpawnWeight
IL_00b7: add
IL_00b8: stloc.1
IL_00b9: ldloca.s V_3
IL_00bb: call System.Boolean System.Collections.Generic.List`1/Enumerator<DirectedSlimeSpawner>::MoveNext()
IL_00c0: brtrue IL_0086
IL_00c5: leave IL_00d6
IL_00ca: ldloc.3
IL_00cb: box System.Collections.Generic.List`1/Enumerator<DirectedSlimeSpawner>
IL_00d0: callvirt System.Void System.IDisposable::Dispose()
IL_00d5: endfinally
IL_00d6: ldloc.0
IL_00d7: callvirt System.Int32 System.Collections.Generic.Dictionary`2<DirectedSlimeSpawner,System.Single>::get_Count()
IL_00dc: ldc.i4.0
IL_00dd: ble IL_0143
IL_00e2: ldloc.1
IL_00e3: ldc.r4 0
IL_00e8: ble.un IL_0143
IL_00ed: ldarg.0
IL_00ee: ldfld Randoms CellDirector::rand
IL_00f3: ldloc.0
IL_00f4: ldnull
IL_00f5: callvirt !!0 Randoms::Pick<DirectedSlimeSpawner>(System.Collections.Generic.IDictionary`2<!!0,System.Single>,!!0)
IL_00fa: stloc.s V_4
IL_00fc: call T SRSingleton`1<SceneContext>::get_Instance()
IL_0101: callvirt ModDirector SceneContext::get_ModDirector()
IL_0106: callvirt System.Single ModDirector::SlimeCountFactor()
IL_010b: stloc.s V_5
IL_010d: ldarg.0
IL_010e: ldloc.s V_4
IL_0110: ldarg.0
IL_0111: ldfld Randoms CellDirector::rand
IL_0116: ldarg.0
IL_0117: ldfld System.Int32 CellDirector::minPerSpawn
IL_011c: ldarg.0
IL_011d: ldfld System.Int32 CellDirector::maxPerSpawn
IL_0122: ldc.i4.1
IL_0123: add
IL_0124: callvirt System.Int32 Randoms::GetInRange(System.Int32,System.Int32)
IL_0129: conv.r4
IL_012a: ldloc.s V_5
IL_012c: mul
IL_012d: call System.Int32 UnityEngine.Mathf::RoundToInt(System.Single)
IL_0132: ldarg.0
IL_0133: ldfld Randoms CellDirector::rand
IL_0138: callvirt System.Collections.IEnumerator DirectedActorSpawner::Spawn(System.Int32,Randoms)
IL_013d: call UnityEngine.Coroutine UnityEngine.MonoBehaviour::StartCoroutine(System.Collections.IEnumerator)
IL_0142: pop
IL_0143: br IL_0171
IL_0148: ldarg.0
IL_0149: call System.Boolean CellDirector::HasTooManySlimes()
IL_014e: brfalse IL_0171
IL_0153: ldarg.0
IL_0154: ldarg.0
IL_0155: ldfld System.Collections.Generic.List`1<UnityEngine.GameObject> CellDirector::slimes
IL_015a: ldc.r4 0.1
IL_015f: ldarg.0
IL_0160: ldfld System.Int32 CellDirector::targetSlimeCount
IL_0165: conv.r4
IL_0166: mul
IL_0167: call System.Int32 UnityEngine.Mathf::CeilToInt(System.Single)
IL_016c: call System.Void CellDirector::Despawn(System.Collections.Generic.List`1<UnityEngine.GameObject>,System.Int32)
IL_0171: ldarg.0
IL_0172: ldfld System.Collections.Generic.List`1<DirectedAnimalSpawner> CellDirector::animalSpawners
IL_0177: callvirt System.Int32 System.Collections.Generic.List`1<DirectedAnimalSpawner>::get_Count()
IL_017c: ldc.i4.0
IL_017d: ble IL_023b
IL_0182: ldarg.0
IL_0183: call System.Boolean CellDirector::NeedsMoreAnimals()
IL_0188: brfalse IL_023b
IL_018d: newobj System.Void System.Collections.Generic.List`1<DirectedAnimalSpawner>::.ctor()
IL_0192: stloc.s V_6
IL_0194: ldarg.0
IL_0195: ldfld System.Collections.Generic.List`1<DirectedAnimalSpawner> CellDirector::animalSpawners
IL_019a: callvirt System.Collections.Generic.List`1/Enumerator<!0> System.Collections.Generic.List`1<DirectedAnimalSpawner>::GetEnumerator()
IL_019f: stloc.s V_8
IL_01a1: br IL_01ce
IL_01a6: ldloca.s V_8
IL_01a8: call !0 System.Collections.Generic.List`1/Enumerator<DirectedAnimalSpawner>::get_Current()
IL_01ad: stloc.s V_7
IL_01af: ldloc.s V_7
IL_01b1: ldloca.s V_11
IL_01b3: initobj System.Nullable`1<System.Single>
IL_01b9: ldloc.s V_11
IL_01bb: callvirt System.Boolean DirectedAnimalSpawner::CanSpawn(System.Nullable`1<System.Single>)
IL_01c0: brfalse IL_01ce
IL_01c5: ldloc.s V_6
IL_01c7: ldloc.s V_7
IL_01c9: callvirt System.Void System.Collections.Generic.List`1<DirectedAnimalSpawner>::Add(!0)
IL_01ce: ldloca.s V_8
IL_01d0: call System.Boolean System.Collections.Generic.List`1/Enumerator<DirectedAnimalSpawner>::MoveNext()
IL_01d5: brtrue IL_01a6
IL_01da: leave IL_01ec
IL_01df: ldloc.s V_8
IL_01e1: box System.Collections.Generic.List`1/Enumerator<DirectedAnimalSpawner>
IL_01e6: callvirt System.Void System.IDisposable::Dispose()
IL_01eb: endfinally
IL_01ec: ldloc.s V_6
IL_01ee: callvirt System.Int32 System.Collections.Generic.List`1<DirectedAnimalSpawner>::get_Count()
IL_01f3: ldc.i4.0
IL_01f4: ble IL_0236
IL_01f9: ldarg.0
IL_01fa: ldfld Randoms CellDirector::rand
IL_01ff: ldloc.s V_6
IL_0201: ldnull
IL_0202: callvirt !!0 Randoms::Pick<DirectedAnimalSpawner>(System.Collections.Generic.ICollection`1<!!0>,!!0)
IL_0207: stloc.s V_9
IL_0209: ldarg.0
IL_020a: ldloc.s V_9
IL_020c: ldarg.0
IL_020d: ldfld Randoms CellDirector::rand
IL_0212: ldarg.0
IL_0213: ldfld System.Int32 CellDirector::minAnimalPerSpawn
IL_0218: ldarg.0
IL_0219: ldfld System.Int32 CellDirector::maxAnimalPerSpawn
IL_021e: ldc.i4.1
IL_021f: add
IL_0220: callvirt System.Int32 Randoms::GetInRange(System.Int32,System.Int32)
IL_0225: ldarg.0
IL_0226: ldfld Randoms CellDirector::rand
IL_022b: callvirt System.Collections.IEnumerator DirectedAnimalSpawner::Spawn(System.Int32,Randoms)
IL_0230: call UnityEngine.Coroutine UnityEngine.MonoBehaviour::StartCoroutine(System.Collections.IEnumerator)
IL_0235: pop
IL_0236: br IL_0264
IL_023b: ldarg.0
IL_023c: call System.Boolean CellDirector::HasTooManyAnimals()
IL_0241: brfalse IL_0264
IL_0246: ldarg.0
IL_0247: ldarg.0
IL_0248: ldfld System.Collections.Generic.List`1<UnityEngine.GameObject> CellDirector::animals
IL_024d: ldc.r4 0.1
IL_0252: ldarg.0
IL_0253: ldfld System.Int32 CellDirector::targetAnimalCount
IL_0258: conv.r4
IL_0259: mul
IL_025a: call System.Int32 UnityEngine.Mathf::CeilToInt(System.Single)
IL_025f: call System.Void CellDirector::Despawn(System.Collections.Generic.List`1<UnityEngine.GameObject>,System.Int32)
IL_0264: ldarg.0
IL_0265: dup
IL_0266: ldfld System.Single CellDirector::nextSpawn
IL_026b: ldarg.0
IL_026c: ldfld System.Single CellDirector::avgSpawnTimeGameHours
IL_0271: ldc.r4 3600
IL_0276: mul
IL_0277: ldarg.0
IL_0278: ldfld Randoms CellDirector::rand
IL_027d: ldc.r4 0.5
IL_0282: ldc.r4 1.5
IL_0287: callvirt System.Single Randoms::GetInRange(System.Single,System.Single)
IL_028c: mul
IL_028d: add
IL_028e: stfld System.Single CellDirector::nextSpawn
IL_0293: ldarg.0
IL_0294: ldfld System.Collections.Generic.List`1<UnityEngine.GameObject> CellDirector::slimes
IL_0299: callvirt System.Int32 System.Collections.Generic.List`1<UnityEngine.GameObject>::get_Count()
IL_029e: ldarg.0
IL_029f: ldfld System.Int32 CellDirector::cullSlimesLimit
IL_02a4: ble IL_02c7
IL_02a9: ldarg.0
IL_02aa: ldarg.0
IL_02ab: ldfld System.Collections.Generic.List`1<UnityEngine.GameObject> CellDirector::slimes
IL_02b0: ldarg.0
IL_02b1: ldfld System.Collections.Generic.List`1<UnityEngine.GameObject> CellDirector::slimes
IL_02b6: callvirt System.Int32 System.Collections.Generic.List`1<UnityEngine.GameObject>::get_Count()
IL_02bb: ldarg.0
IL_02bc: ldfld System.Int32 CellDirector::cullSlimesLimit
IL_02c1: sub
IL_02c2: call System.Void CellDirector::Despawn(System.Collections.Generic.List`1<UnityEngine.GameObject>,System.Int32)
IL_02c7: ldarg.0
IL_02c8: ldfld System.Collections.Generic.List`1<UnityEngine.GameObject> CellDirector::animals
IL_02cd: callvirt System.Int32 System.Collections.Generic.List`1<UnityEngine.GameObject>::get_Count()
IL_02d2: ldarg.0
IL_02d3: ldfld System.Int32 CellDirector::cullAnimalsLimit
IL_02d8: ble IL_02fb
IL_02dd: ldarg.0
IL_02de: ldarg.0
IL_02df: ldfld System.Collections.Generic.List`1<UnityEngine.GameObject> CellDirector::animals
IL_02e4: ldarg.0
IL_02e5: ldfld System.Collections.Generic.List`1<UnityEngine.GameObject> CellDirector::animals
IL_02ea: callvirt System.Int32 System.Collections.Generic.List`1<UnityEngine.GameObject>::get_Count()
IL_02ef: ldarg.0
IL_02f0: ldfld System.Int32 CellDirector::cullAnimalsLimit
IL_02f5: sub
IL_02f6: call System.Void CellDirector::Despawn(System.Collections.Generic.List`1<UnityEngine.GameObject>,System.Int32)
IL_02fb: ldarg.0
IL_02fc: call System.Single UnityEngine.Time::get_time()
IL_0301: ldc.r4 1
IL_0306: add
IL_0307: stfld System.Single CellDirector::spawnThrottleTime
IL_030c: ret

*/
//  CellDirector.Update
/*
IL_0000: ldarg.0
IL_0001: ldfld System.Boolean CellDirector::allowSpawns
IL_0006: brtrue IL_000c
IL_000b: ret
IL_000c: call System.Single UnityEngine.Time::get_time()
IL_0011: ldarg.0
IL_0012: ldfld System.Single CellDirector::spawnThrottleTime
IL_0017: blt.un IL_030c
IL_001c: ldarg.0
IL_001d: ldfld TimeDirector CellDirector::timeDir
IL_0022: ldarg.0
IL_0023: ldfld System.Single CellDirector::nextSpawn
IL_0028: callvirt System.Boolean TimeDirector::HasReached(System.Single)
IL_002d: brfalse IL_0293
IL_0032: ldarg.0
IL_0033: ldfld SECTR_Sector CellDirector::sector
IL_0038: callvirt System.Boolean SECTR_Member::get_Hibernate()
IL_003d: brtrue IL_0293
IL_0042: ldarg.0
IL_0043: ldfld System.Collections.Generic.List`1<DirectedSlimeSpawner> CellDirector::spawners
IL_0048: callvirt System.Int32 System.Collections.Generic.List`1<DirectedSlimeSpawner>::get_Count()
IL_004d: ldc.i4.0
IL_004e: ble IL_0148
IL_0053: ldarg.0
IL_0054: call System.Boolean CellDirector::NeedsMoreSlimes()
IL_0059: brfalse IL_0148
IL_005e: ldarg.0
IL_005f: call System.Boolean CellDirector::HasTarrSlimes()
IL_0064: brtrue IL_0148
IL_0069: newobj System.Void System.Collections.Generic.Dictionary`2<DirectedSlimeSpawner,System.Single>::.ctor()
IL_006e: stloc.0
IL_006f: ldc.r4 0
IL_0074: stloc.1
IL_0075: ldarg.0
IL_0076: ldfld System.Collections.Generic.List`1<DirectedSlimeSpawner> CellDirector::spawners
IL_007b: callvirt System.Collections.Generic.List`1/Enumerator<!0> System.Collections.Generic.List`1<DirectedSlimeSpawner>::GetEnumerator()
IL_0080: stloc.3
IL_0081: br IL_00b9
IL_0086: ldloca.s V_3
IL_0088: call !0 System.Collections.Generic.List`1/Enumerator<DirectedSlimeSpawner>::get_Current()
IL_008d: stloc.2
IL_008e: ldloc.2
IL_008f: ldloca.s V_10
IL_0091: initobj System.Nullable`1<System.Single>
IL_0097: ldloc.s V_10
IL_0099: callvirt System.Boolean DirectedActorSpawner::CanSpawn(System.Nullable`1<System.Single>)
IL_009e: brfalse IL_00b9
IL_00a3: ldloc.0
IL_00a4: ldloc.2
IL_00a5: ldloc.2
IL_00a6: ldfld System.Single DirectedActorSpawner::directedSpawnWeight
IL_00ab: callvirt System.Void System.Collections.Generic.Dictionary`2<DirectedSlimeSpawner,System.Single>::set_Item(!0,!1)
IL_00b0: ldloc.1
IL_00b1: ldloc.2
IL_00b2: ldfld System.Single DirectedActorSpawner::directedSpawnWeight
IL_00b7: add
IL_00b8: stloc.1
IL_00b9: ldloca.s V_3
IL_00bb: call System.Boolean System.Collections.Generic.List`1/Enumerator<DirectedSlimeSpawner>::MoveNext()
IL_00c0: brtrue IL_0086
IL_00c5: leave IL_00d6
IL_00ca: ldloc.3
IL_00cb: box System.Collections.Generic.List`1/Enumerator<DirectedSlimeSpawner>
IL_00d0: callvirt System.Void System.IDisposable::Dispose()
IL_00d5: endfinally
IL_00d6: ldloc.0
IL_00d7: callvirt System.Int32 System.Collections.Generic.Dictionary`2<DirectedSlimeSpawner,System.Single>::get_Count()
IL_00dc: ldc.i4.0
IL_00dd: ble IL_0143
IL_00e2: ldloc.1
IL_00e3: ldc.r4 0
IL_00e8: ble.un IL_0143
IL_00ed: ldarg.0
IL_00ee: ldfld Randoms CellDirector::rand
IL_00f3: ldloc.0
IL_00f4: ldnull
IL_00f5: callvirt !!0 Randoms::Pick<DirectedSlimeSpawner>(System.Collections.Generic.IDictionary`2<!!0,System.Single>,!!0)
IL_00fa: stloc.s V_4
IL_00fc: call T SRSingleton`1<SceneContext>::get_Instance()
IL_0101: callvirt ModDirector SceneContext::get_ModDirector()
IL_0106: callvirt System.Single ModDirector::SlimeCountFactor()
IL_010b: stloc.s V_5
IL_010d: ldarg.0
IL_010e: ldloc.s V_4
IL_0110: ldarg.0
IL_0111: ldfld Randoms CellDirector::rand
IL_0116: ldarg.0
IL_0117: ldfld System.Int32 CellDirector::minPerSpawn
IL_011c: ldarg.0
IL_011d: ldfld System.Int32 CellDirector::maxPerSpawn
IL_0122: ldc.i4.1
IL_0123: add
IL_0124: callvirt System.Int32 Randoms::GetInRange(System.Int32,System.Int32)
IL_0129: conv.r4
IL_012a: ldloc.s V_5
IL_012c: mul
IL_012d: call System.Int32 UnityEngine.Mathf::RoundToInt(System.Single)
IL_0132: ldarg.0
IL_0133: ldfld Randoms CellDirector::rand
IL_0138: callvirt System.Collections.IEnumerator DirectedActorSpawner::Spawn(System.Int32,Randoms)
IL_013d: call UnityEngine.Coroutine UnityEngine.MonoBehaviour::StartCoroutine(System.Collections.IEnumerator)
IL_0142: pop
IL_0143: br IL_0171
IL_0148: ldarg.0
IL_0149: call System.Boolean CellDirector::HasTooManySlimes()
IL_014e: brfalse IL_0171
IL_0153: ldarg.0
IL_0154: ldarg.0
IL_0155: ldfld System.Collections.Generic.List`1<UnityEngine.GameObject> CellDirector::slimes
IL_015a: ldc.r4 0.1
IL_015f: ldarg.0
IL_0160: ldfld System.Int32 CellDirector::targetSlimeCount
IL_0165: conv.r4
IL_0166: mul
IL_0167: call System.Int32 UnityEngine.Mathf::CeilToInt(System.Single)
IL_016c: call System.Void CellDirector::Despawn(System.Collections.Generic.List`1<UnityEngine.GameObject>,System.Int32)
IL_0171: ldarg.0
IL_0172: ldfld System.Collections.Generic.List`1<DirectedAnimalSpawner> CellDirector::animalSpawners
IL_0177: callvirt System.Int32 System.Collections.Generic.List`1<DirectedAnimalSpawner>::get_Count()
IL_017c: ldc.i4.0
IL_017d: ble IL_023b
IL_0182: ldarg.0
IL_0183: call System.Boolean CellDirector::NeedsMoreAnimals()
IL_0188: brfalse IL_023b
IL_018d: newobj System.Void System.Collections.Generic.List`1<DirectedAnimalSpawner>::.ctor()
IL_0192: stloc.s V_6
IL_0194: ldarg.0
IL_0195: ldfld System.Collections.Generic.List`1<DirectedAnimalSpawner> CellDirector::animalSpawners
IL_019a: callvirt System.Collections.Generic.List`1/Enumerator<!0> System.Collections.Generic.List`1<DirectedAnimalSpawner>::GetEnumerator()
IL_019f: stloc.s V_8
IL_01a1: br IL_01ce
IL_01a6: ldloca.s V_8
IL_01a8: call !0 System.Collections.Generic.List`1/Enumerator<DirectedAnimalSpawner>::get_Current()
IL_01ad: stloc.s V_7
IL_01af: ldloc.s V_7
IL_01b1: ldloca.s V_11
IL_01b3: initobj System.Nullable`1<System.Single>
IL_01b9: ldloc.s V_11
IL_01bb: callvirt System.Boolean DirectedAnimalSpawner::CanSpawn(System.Nullable`1<System.Single>)
IL_01c0: brfalse IL_01ce
IL_01c5: ldloc.s V_6
IL_01c7: ldloc.s V_7
IL_01c9: callvirt System.Void System.Collections.Generic.List`1<DirectedAnimalSpawner>::Add(!0)
IL_01ce: ldloca.s V_8
IL_01d0: call System.Boolean System.Collections.Generic.List`1/Enumerator<DirectedAnimalSpawner>::MoveNext()
IL_01d5: brtrue IL_01a6
IL_01da: leave IL_01ec
IL_01df: ldloc.s V_8
IL_01e1: box System.Collections.Generic.List`1/Enumerator<DirectedAnimalSpawner>
IL_01e6: callvirt System.Void System.IDisposable::Dispose()
IL_01eb: endfinally
IL_01ec: ldloc.s V_6
IL_01ee: callvirt System.Int32 System.Collections.Generic.List`1<DirectedAnimalSpawner>::get_Count()
IL_01f3: ldc.i4.0
IL_01f4: ble IL_0236
IL_01f9: ldarg.0
IL_01fa: ldfld Randoms CellDirector::rand
IL_01ff: ldloc.s V_6
IL_0201: ldnull
IL_0202: callvirt !!0 Randoms::Pick<DirectedAnimalSpawner>(System.Collections.Generic.ICollection`1<!!0>,!!0)
IL_0207: stloc.s V_9
IL_0209: ldarg.0
IL_020a: ldloc.s V_9
IL_020c: ldarg.0
IL_020d: ldfld Randoms CellDirector::rand
IL_0212: ldarg.0
IL_0213: ldfld System.Int32 CellDirector::minAnimalPerSpawn
IL_0218: ldarg.0
IL_0219: ldfld System.Int32 CellDirector::maxAnimalPerSpawn
IL_021e: ldc.i4.1
IL_021f: add
IL_0220: callvirt System.Int32 Randoms::GetInRange(System.Int32,System.Int32)
IL_0225: ldarg.0
IL_0226: ldfld Randoms CellDirector::rand
IL_022b: callvirt System.Collections.IEnumerator DirectedAnimalSpawner::Spawn(System.Int32,Randoms)
IL_0230: call UnityEngine.Coroutine UnityEngine.MonoBehaviour::StartCoroutine(System.Collections.IEnumerator)
IL_0235: pop
IL_0236: br IL_0264
IL_023b: ldarg.0
IL_023c: call System.Boolean CellDirector::HasTooManyAnimals()
IL_0241: brfalse IL_0264
IL_0246: ldarg.0
IL_0247: ldarg.0
IL_0248: ldfld System.Collections.Generic.List`1<UnityEngine.GameObject> CellDirector::animals
IL_024d: ldc.r4 0.1
IL_0252: ldarg.0
IL_0253: ldfld System.Int32 CellDirector::targetAnimalCount
IL_0258: conv.r4
IL_0259: mul
IL_025a: call System.Int32 UnityEngine.Mathf::CeilToInt(System.Single)
IL_025f: call System.Void CellDirector::Despawn(System.Collections.Generic.List`1<UnityEngine.GameObject>,System.Int32)
IL_0264: ldarg.0
IL_0265: dup
IL_0266: ldfld System.Single CellDirector::nextSpawn
IL_026b: ldarg.0
IL_026c: ldfld System.Single CellDirector::avgSpawnTimeGameHours
IL_0271: ldc.r4 3600
IL_0276: mul
IL_0277: ldarg.0
IL_0278: ldfld Randoms CellDirector::rand
IL_027d: ldc.r4 0.5
IL_0282: ldc.r4 1.5
IL_0287: callvirt System.Single Randoms::GetInRange(System.Single,System.Single)
IL_028c: mul
IL_028d: add
IL_028e: stfld System.Single CellDirector::nextSpawn
IL_0293: ldarg.0
IL_0294: ldfld System.Collections.Generic.List`1<UnityEngine.GameObject> CellDirector::slimes
IL_0299: callvirt System.Int32 System.Collections.Generic.List`1<UnityEngine.GameObject>::get_Count()
IL_029e: ldarg.0
IL_029f: ldfld System.Int32 CellDirector::cullSlimesLimit
IL_02a4: ble IL_02c7
IL_02a9: ldarg.0
IL_02aa: ldarg.0
IL_02ab: ldfld System.Collections.Generic.List`1<UnityEngine.GameObject> CellDirector::slimes
IL_02b0: ldarg.0
IL_02b1: ldfld System.Collections.Generic.List`1<UnityEngine.GameObject> CellDirector::slimes
IL_02b6: callvirt System.Int32 System.Collections.Generic.List`1<UnityEngine.GameObject>::get_Count()
IL_02bb: ldarg.0
IL_02bc: ldfld System.Int32 CellDirector::cullSlimesLimit
IL_02c1: sub
IL_02c2: call System.Void CellDirector::Despawn(System.Collections.Generic.List`1<UnityEngine.GameObject>,System.Int32)
IL_02c7: ldarg.0
IL_02c8: ldfld System.Collections.Generic.List`1<UnityEngine.GameObject> CellDirector::animals
IL_02cd: callvirt System.Int32 System.Collections.Generic.List`1<UnityEngine.GameObject>::get_Count()
IL_02d2: ldarg.0
IL_02d3: ldfld System.Int32 CellDirector::cullAnimalsLimit
IL_02d8: ble IL_02fb
IL_02dd: ldarg.0
IL_02de: ldarg.0
IL_02df: ldfld System.Collections.Generic.List`1<UnityEngine.GameObject> CellDirector::animals
IL_02e4: ldarg.0
IL_02e5: ldfld System.Collections.Generic.List`1<UnityEngine.GameObject> CellDirector::animals
IL_02ea: callvirt System.Int32 System.Collections.Generic.List`1<UnityEngine.GameObject>::get_Count()
IL_02ef: ldarg.0
IL_02f0: ldfld System.Int32 CellDirector::cullAnimalsLimit
IL_02f5: sub
IL_02f6: call System.Void CellDirector::Despawn(System.Collections.Generic.List`1<UnityEngine.GameObject>,System.Int32)
IL_02fb: ldarg.0
IL_02fc: call System.Single UnityEngine.Time::get_time()
IL_0301: ldc.r4 1
IL_0306: add
IL_0307: stfld System.Single CellDirector::spawnThrottleTime
IL_030c: ret

*/
//  LandPlot.SetUpgrades
/*
IL_0000: ldarg.0
IL_0001: ldarg.1
IL_0002: stfld System.Collections.Generic.List`1<LandPlot/Upgrade> LandPlot::upgrades
IL_0007: ldarg.1
IL_0008: callvirt System.Collections.Generic.List`1/Enumerator<!0> System.Collections.Generic.List`1<LandPlot/Upgrade>::GetEnumerator()
IL_000d: stloc.1
IL_000e: br IL_0022
IL_0013: ldloca.s V_1
IL_0015: call !0 System.Collections.Generic.List`1/Enumerator<LandPlot/Upgrade>::get_Current()
IL_001a: stloc.0
IL_001b: ldarg.0
IL_001c: ldloc.0
IL_001d: call System.Void LandPlot::ApplyUpgrade(LandPlot/Upgrade)
IL_0022: ldloca.s V_1
IL_0024: call System.Boolean System.Collections.Generic.List`1/Enumerator<LandPlot/Upgrade>::MoveNext()
IL_0029: brtrue IL_0013
IL_002e: leave IL_003f
IL_0033: ldloc.1
IL_0034: box System.Collections.Generic.List`1/Enumerator<LandPlot/Upgrade>
IL_0039: callvirt System.Void System.IDisposable::Dispose()
IL_003e: endfinally
IL_003f: ret

*/
//  PersonalUpgradeUI.CreatePurchaseUI
/*
IL_0000: ldc.i4.s 14
IL_0002: newarr PurchaseUI/Purchasable
IL_0007: dup
IL_0008: ldc.i4.0
IL_0009: ldstr "m.upgrade.name.personal.liquid_slot"
IL_000e: ldarg.0
IL_000f: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::liquidSlot
IL_0014: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::icon
IL_0019: ldarg.0
IL_001a: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::liquidSlot
IL_001f: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::img
IL_0024: ldstr "m.upgrade.desc.personal.liquid_slot"
IL_0029: ldarg.0
IL_002a: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::liquidSlot
IL_002f: ldfld System.Int32 PersonalUpgradeUI/UpgradePurchaseItem::cost
IL_0034: ldloca.s V_1
IL_0036: initobj System.Nullable`1<PediaDirector/Id>
IL_003c: ldloc.1
IL_003d: ldarg.0
IL_003e: ldftn System.Void PersonalUpgradeUI::UpgradeLiquidSlot()
IL_0044: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_0049: ldarg.0
IL_004a: ldfld PlayerState PersonalUpgradeUI::playerState
IL_004f: ldc.i4.s 13
IL_0051: callvirt System.Boolean PlayerState::CanGetUpgrade(PlayerState/Upgrade)
IL_0056: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_005b: stelem.ref
IL_005c: dup
IL_005d: ldc.i4.1
IL_005e: ldstr "m.upgrade.name.personal.jetpack"
IL_0063: ldarg.0
IL_0064: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::jetpack
IL_0069: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::icon
IL_006e: ldarg.0
IL_006f: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::jetpack
IL_0074: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::img
IL_0079: ldstr "m.upgrade.desc.personal.jetpack"
IL_007e: ldarg.0
IL_007f: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::jetpack
IL_0084: ldfld System.Int32 PersonalUpgradeUI/UpgradePurchaseItem::cost
IL_0089: ldloca.s V_2
IL_008b: initobj System.Nullable`1<PediaDirector/Id>
IL_0091: ldloc.2
IL_0092: ldarg.0
IL_0093: ldftn System.Void PersonalUpgradeUI::UpgradeJetpack()
IL_0099: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_009e: ldarg.0
IL_009f: ldfld PlayerState PersonalUpgradeUI::playerState
IL_00a4: ldc.i4.s 9
IL_00a6: callvirt System.Boolean PlayerState::CanGetUpgrade(PlayerState/Upgrade)
IL_00ab: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_00b0: stelem.ref
IL_00b1: dup
IL_00b2: ldc.i4.2
IL_00b3: ldstr "m.upgrade.name.personal.jetpack_efficiency"
IL_00b8: ldarg.0
IL_00b9: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::jetpackEfficiency
IL_00be: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::icon
IL_00c3: ldarg.0
IL_00c4: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::jetpackEfficiency
IL_00c9: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::img
IL_00ce: ldstr "m.upgrade.desc.personal.jetpack_efficiency"
IL_00d3: ldarg.0
IL_00d4: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::jetpackEfficiency
IL_00d9: ldfld System.Int32 PersonalUpgradeUI/UpgradePurchaseItem::cost
IL_00de: ldloca.s V_3
IL_00e0: initobj System.Nullable`1<PediaDirector/Id>
IL_00e6: ldloc.3
IL_00e7: ldarg.0
IL_00e8: ldftn System.Void PersonalUpgradeUI::UpgradeJetpackEfficiency()
IL_00ee: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_00f3: ldarg.0
IL_00f4: ldfld PlayerState PersonalUpgradeUI::playerState
IL_00f9: ldc.i4.s 10
IL_00fb: callvirt System.Boolean PlayerState::CanGetUpgrade(PlayerState/Upgrade)
IL_0100: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_0105: stelem.ref
IL_0106: dup
IL_0107: ldc.i4.3
IL_0108: ldstr "m.upgrade.name.personal.run_efficiency"
IL_010d: ldarg.0
IL_010e: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::runEfficiency
IL_0113: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::icon
IL_0118: ldarg.0
IL_0119: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::runEfficiency
IL_011e: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::img
IL_0123: ldstr "m.upgrade.desc.personal.run_efficiency"
IL_0128: ldarg.0
IL_0129: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::runEfficiency
IL_012e: ldfld System.Int32 PersonalUpgradeUI/UpgradePurchaseItem::cost
IL_0133: ldloca.s V_4
IL_0135: initobj System.Nullable`1<PediaDirector/Id>
IL_013b: ldloc.s V_4
IL_013d: ldarg.0
IL_013e: ldftn System.Void PersonalUpgradeUI::UpgradeRunEfficiency()
IL_0144: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_0149: ldarg.0
IL_014a: ldfld PlayerState PersonalUpgradeUI::playerState
IL_014f: ldc.i4.s 12
IL_0151: callvirt System.Boolean PlayerState::CanGetUpgrade(PlayerState/Upgrade)
IL_0156: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_015b: stelem.ref
IL_015c: dup
IL_015d: ldc.i4.4
IL_015e: ldstr "m.upgrade.name.personal.air_burst"
IL_0163: ldarg.0
IL_0164: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::airBurst
IL_0169: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::icon
IL_016e: ldarg.0
IL_016f: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::airBurst
IL_0174: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::img
IL_0179: ldstr "m.upgrade.desc.personal.air_burst"
IL_017e: ldarg.0
IL_017f: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::airBurst
IL_0184: ldfld System.Int32 PersonalUpgradeUI/UpgradePurchaseItem::cost
IL_0189: ldloca.s V_5
IL_018b: initobj System.Nullable`1<PediaDirector/Id>
IL_0191: ldloc.s V_5
IL_0193: ldarg.0
IL_0194: ldftn System.Void PersonalUpgradeUI::UpgradeAirBurst()
IL_019a: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_019f: ldarg.0
IL_01a0: ldfld PlayerState PersonalUpgradeUI::playerState
IL_01a5: ldc.i4.s 11
IL_01a7: callvirt System.Boolean PlayerState::CanGetUpgrade(PlayerState/Upgrade)
IL_01ac: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_01b1: stelem.ref
IL_01b2: dup
IL_01b3: ldc.i4.5
IL_01b4: ldstr "m.upgrade.name.personal.health1"
IL_01b9: ldarg.0
IL_01ba: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::health1
IL_01bf: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::icon
IL_01c4: ldarg.0
IL_01c5: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::health1
IL_01ca: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::img
IL_01cf: ldstr "m.upgrade.desc.personal.health1"
IL_01d4: ldarg.0
IL_01d5: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::health1
IL_01da: ldfld System.Int32 PersonalUpgradeUI/UpgradePurchaseItem::cost
IL_01df: ldloca.s V_6
IL_01e1: initobj System.Nullable`1<PediaDirector/Id>
IL_01e7: ldloc.s V_6
IL_01e9: ldarg.0
IL_01ea: ldftn System.Void PersonalUpgradeUI::UpgradeHealth1()
IL_01f0: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_01f5: ldarg.0
IL_01f6: ldfld PlayerState PersonalUpgradeUI::playerState
IL_01fb: ldc.i4.0
IL_01fc: callvirt System.Boolean PlayerState::CanGetUpgrade(PlayerState/Upgrade)
IL_0201: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_0206: stelem.ref
IL_0207: dup
IL_0208: ldc.i4.6
IL_0209: ldstr "m.upgrade.name.personal.health2"
IL_020e: ldarg.0
IL_020f: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::health2
IL_0214: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::icon
IL_0219: ldarg.0
IL_021a: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::health2
IL_021f: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::img
IL_0224: ldstr "m.upgrade.desc.personal.health2"
IL_0229: ldarg.0
IL_022a: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::health2
IL_022f: ldfld System.Int32 PersonalUpgradeUI/UpgradePurchaseItem::cost
IL_0234: ldloca.s V_7
IL_0236: initobj System.Nullable`1<PediaDirector/Id>
IL_023c: ldloc.s V_7
IL_023e: ldarg.0
IL_023f: ldftn System.Void PersonalUpgradeUI::UpgradeHealth2()
IL_0245: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_024a: ldarg.0
IL_024b: ldfld PlayerState PersonalUpgradeUI::playerState
IL_0250: ldc.i4.1
IL_0251: callvirt System.Boolean PlayerState::CanGetUpgrade(PlayerState/Upgrade)
IL_0256: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_025b: stelem.ref
IL_025c: dup
IL_025d: ldc.i4.7
IL_025e: ldstr "m.upgrade.name.personal.health3"
IL_0263: ldarg.0
IL_0264: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::health3
IL_0269: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::icon
IL_026e: ldarg.0
IL_026f: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::health3
IL_0274: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::img
IL_0279: ldstr "m.upgrade.desc.personal.health3"
IL_027e: ldarg.0
IL_027f: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::health3
IL_0284: ldfld System.Int32 PersonalUpgradeUI/UpgradePurchaseItem::cost
IL_0289: ldloca.s V_8
IL_028b: initobj System.Nullable`1<PediaDirector/Id>
IL_0291: ldloc.s V_8
IL_0293: ldarg.0
IL_0294: ldftn System.Void PersonalUpgradeUI::UpgradeHealth3()
IL_029a: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_029f: ldarg.0
IL_02a0: ldfld PlayerState PersonalUpgradeUI::playerState
IL_02a5: ldc.i4.2
IL_02a6: callvirt System.Boolean PlayerState::CanGetUpgrade(PlayerState/Upgrade)
IL_02ab: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_02b0: stelem.ref
IL_02b1: dup
IL_02b2: ldc.i4.8
IL_02b3: ldstr "m.upgrade.name.personal.energy1"
IL_02b8: ldarg.0
IL_02b9: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::energy1
IL_02be: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::icon
IL_02c3: ldarg.0
IL_02c4: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::energy1
IL_02c9: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::img
IL_02ce: ldstr "m.upgrade.desc.personal.energy1"
IL_02d3: ldarg.0
IL_02d4: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::energy1
IL_02d9: ldfld System.Int32 PersonalUpgradeUI/UpgradePurchaseItem::cost
IL_02de: ldloca.s V_9
IL_02e0: initobj System.Nullable`1<PediaDirector/Id>
IL_02e6: ldloc.s V_9
IL_02e8: ldarg.0
IL_02e9: ldftn System.Void PersonalUpgradeUI::UpgradeEnergy1()
IL_02ef: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_02f4: ldarg.0
IL_02f5: ldfld PlayerState PersonalUpgradeUI::playerState
IL_02fa: ldc.i4.3
IL_02fb: callvirt System.Boolean PlayerState::CanGetUpgrade(PlayerState/Upgrade)
IL_0300: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_0305: stelem.ref
IL_0306: dup
IL_0307: ldc.i4.s 9
IL_0309: ldstr "m.upgrade.name.personal.energy2"
IL_030e: ldarg.0
IL_030f: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::energy2
IL_0314: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::icon
IL_0319: ldarg.0
IL_031a: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::energy2
IL_031f: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::img
IL_0324: ldstr "m.upgrade.desc.personal.energy2"
IL_0329: ldarg.0
IL_032a: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::energy2
IL_032f: ldfld System.Int32 PersonalUpgradeUI/UpgradePurchaseItem::cost
IL_0334: ldloca.s V_10
IL_0336: initobj System.Nullable`1<PediaDirector/Id>
IL_033c: ldloc.s V_10
IL_033e: ldarg.0
IL_033f: ldftn System.Void PersonalUpgradeUI::UpgradeEnergy2()
IL_0345: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_034a: ldarg.0
IL_034b: ldfld PlayerState PersonalUpgradeUI::playerState
IL_0350: ldc.i4.4
IL_0351: callvirt System.Boolean PlayerState::CanGetUpgrade(PlayerState/Upgrade)
IL_0356: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_035b: stelem.ref
IL_035c: dup
IL_035d: ldc.i4.s 10
IL_035f: ldstr "m.upgrade.name.personal.energy3"
IL_0364: ldarg.0
IL_0365: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::energy3
IL_036a: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::icon
IL_036f: ldarg.0
IL_0370: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::energy3
IL_0375: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::img
IL_037a: ldstr "m.upgrade.desc.personal.energy3"
IL_037f: ldarg.0
IL_0380: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::energy3
IL_0385: ldfld System.Int32 PersonalUpgradeUI/UpgradePurchaseItem::cost
IL_038a: ldloca.s V_11
IL_038c: initobj System.Nullable`1<PediaDirector/Id>
IL_0392: ldloc.s V_11
IL_0394: ldarg.0
IL_0395: ldftn System.Void PersonalUpgradeUI::UpgradeEnergy3()
IL_039b: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_03a0: ldarg.0
IL_03a1: ldfld PlayerState PersonalUpgradeUI::playerState
IL_03a6: ldc.i4.5
IL_03a7: callvirt System.Boolean PlayerState::CanGetUpgrade(PlayerState/Upgrade)
IL_03ac: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_03b1: stelem.ref
IL_03b2: dup
IL_03b3: ldc.i4.s 11
IL_03b5: ldstr "m.upgrade.name.personal.ammo1"
IL_03ba: ldarg.0
IL_03bb: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::ammo1
IL_03c0: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::icon
IL_03c5: ldarg.0
IL_03c6: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::ammo1
IL_03cb: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::img
IL_03d0: ldstr "m.upgrade.desc.personal.ammo1"
IL_03d5: ldarg.0
IL_03d6: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::ammo1
IL_03db: ldfld System.Int32 PersonalUpgradeUI/UpgradePurchaseItem::cost
IL_03e0: ldloca.s V_12
IL_03e2: initobj System.Nullable`1<PediaDirector/Id>
IL_03e8: ldloc.s V_12
IL_03ea: ldarg.0
IL_03eb: ldftn System.Void PersonalUpgradeUI::UpgradeAmmo1()
IL_03f1: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_03f6: ldarg.0
IL_03f7: ldfld PlayerState PersonalUpgradeUI::playerState
IL_03fc: ldc.i4.6
IL_03fd: callvirt System.Boolean PlayerState::CanGetUpgrade(PlayerState/Upgrade)
IL_0402: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_0407: stelem.ref
IL_0408: dup
IL_0409: ldc.i4.s 12
IL_040b: ldstr "m.upgrade.name.personal.ammo2"
IL_0410: ldarg.0
IL_0411: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::ammo2
IL_0416: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::icon
IL_041b: ldarg.0
IL_041c: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::ammo2
IL_0421: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::img
IL_0426: ldstr "m.upgrade.desc.personal.ammo2"
IL_042b: ldarg.0
IL_042c: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::ammo2
IL_0431: ldfld System.Int32 PersonalUpgradeUI/UpgradePurchaseItem::cost
IL_0436: ldloca.s V_13
IL_0438: initobj System.Nullable`1<PediaDirector/Id>
IL_043e: ldloc.s V_13
IL_0440: ldarg.0
IL_0441: ldftn System.Void PersonalUpgradeUI::UpgradeAmmo2()
IL_0447: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_044c: ldarg.0
IL_044d: ldfld PlayerState PersonalUpgradeUI::playerState
IL_0452: ldc.i4.7
IL_0453: callvirt System.Boolean PlayerState::CanGetUpgrade(PlayerState/Upgrade)
IL_0458: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_045d: stelem.ref
IL_045e: dup
IL_045f: ldc.i4.s 13
IL_0461: ldstr "m.upgrade.name.personal.ammo3"
IL_0466: ldarg.0
IL_0467: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::ammo3
IL_046c: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::icon
IL_0471: ldarg.0
IL_0472: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::ammo3
IL_0477: ldfld UnityEngine.Sprite PersonalUpgradeUI/UpgradePurchaseItem::img
IL_047c: ldstr "m.upgrade.desc.personal.ammo3"
IL_0481: ldarg.0
IL_0482: ldfld PersonalUpgradeUI/UpgradePurchaseItem PersonalUpgradeUI::ammo3
IL_0487: ldfld System.Int32 PersonalUpgradeUI/UpgradePurchaseItem::cost
IL_048c: ldloca.s V_14
IL_048e: initobj System.Nullable`1<PediaDirector/Id>
IL_0494: ldloc.s V_14
IL_0496: ldarg.0
IL_0497: ldftn System.Void PersonalUpgradeUI::UpgradeAmmo3()
IL_049d: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_04a2: ldarg.0
IL_04a3: ldfld PlayerState PersonalUpgradeUI::playerState
IL_04a8: ldc.i4.8
IL_04a9: callvirt System.Boolean PlayerState::CanGetUpgrade(PlayerState/Upgrade)
IL_04ae: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_04b3: stelem.ref
IL_04b4: stloc.0
IL_04b5: call T SRSingleton`1<GameContext>::get_Instance()
IL_04ba: callvirt UITemplates GameContext::get_UITemplates()
IL_04bf: ldarg.0
IL_04c0: ldfld UnityEngine.Sprite PersonalUpgradeUI::titleIcon
IL_04c5: ldstr "ui"
IL_04ca: ldstr "t.personal_upgrades"
IL_04cf: call System.String MessageUtil::Qualify(System.String,System.String)
IL_04d4: ldloc.0
IL_04d5: ldarg.0
IL_04d6: dup
IL_04d7: ldvirtftn System.Void BaseUI::Close()
IL_04dd: newobj System.Void PurchaseUI/OnClose::.ctor(System.Object,System.IntPtr)
IL_04e2: callvirt UnityEngine.GameObject UITemplates::CreatePurchaseUI(UnityEngine.Sprite,System.String,PurchaseUI/Purchasable[],PurchaseUI/OnClose)
IL_04e7: ret

*/
//  EmptyPlotUI.CreatePurchaseUI
/*
IL_0000: ldc.i4.6
IL_0001: newarr PurchaseUI/Purchasable
IL_0006: dup
IL_0007: ldc.i4.0
IL_0008: ldstr "t.corral"
IL_000d: ldarg.0
IL_000e: ldfld LandPlotUI/PlotPurchaseItem EmptyPlotUI::corral
IL_0013: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_0018: ldarg.0
IL_0019: ldfld LandPlotUI/PlotPurchaseItem EmptyPlotUI::corral
IL_001e: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_0023: ldstr "m.intro.corral"
IL_0028: ldarg.0
IL_0029: ldfld LandPlotUI/PlotPurchaseItem EmptyPlotUI::corral
IL_002e: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_0033: ldc.i4 4000
IL_0038: newobj System.Void System.Nullable`1<PediaDirector/Id>::.ctor(!0)
IL_003d: ldarg.0
IL_003e: ldftn System.Void EmptyPlotUI::BuyCorral()
IL_0044: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_0049: ldc.i4.1
IL_004a: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_004f: stelem.ref
IL_0050: dup
IL_0051: ldc.i4.1
IL_0052: ldstr "t.garden"
IL_0057: ldarg.0
IL_0058: ldfld LandPlotUI/PlotPurchaseItem EmptyPlotUI::garden
IL_005d: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_0062: ldarg.0
IL_0063: ldfld LandPlotUI/PlotPurchaseItem EmptyPlotUI::garden
IL_0068: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_006d: ldstr "m.intro.garden"
IL_0072: ldarg.0
IL_0073: ldfld LandPlotUI/PlotPurchaseItem EmptyPlotUI::garden
IL_0078: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_007d: ldc.i4 4002
IL_0082: newobj System.Void System.Nullable`1<PediaDirector/Id>::.ctor(!0)
IL_0087: ldarg.0
IL_0088: ldftn System.Void EmptyPlotUI::BuyGarden()
IL_008e: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_0093: ldc.i4.1
IL_0094: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_0099: stelem.ref
IL_009a: dup
IL_009b: ldc.i4.2
IL_009c: ldstr "t.coop"
IL_00a1: ldarg.0
IL_00a2: ldfld LandPlotUI/PlotPurchaseItem EmptyPlotUI::coop
IL_00a7: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_00ac: ldarg.0
IL_00ad: ldfld LandPlotUI/PlotPurchaseItem EmptyPlotUI::coop
IL_00b2: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_00b7: ldstr "m.intro.coop"
IL_00bc: ldarg.0
IL_00bd: ldfld LandPlotUI/PlotPurchaseItem EmptyPlotUI::coop
IL_00c2: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_00c7: ldc.i4 4001
IL_00cc: newobj System.Void System.Nullable`1<PediaDirector/Id>::.ctor(!0)
IL_00d1: ldarg.0
IL_00d2: ldftn System.Void EmptyPlotUI::BuyCoop()
IL_00d8: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_00dd: ldc.i4.1
IL_00de: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_00e3: stelem.ref
IL_00e4: dup
IL_00e5: ldc.i4.3
IL_00e6: ldstr "t.silo"
IL_00eb: ldarg.0
IL_00ec: ldfld LandPlotUI/PlotPurchaseItem EmptyPlotUI::silo
IL_00f1: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_00f6: ldarg.0
IL_00f7: ldfld LandPlotUI/PlotPurchaseItem EmptyPlotUI::silo
IL_00fc: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_0101: ldstr "m.intro.silo"
IL_0106: ldarg.0
IL_0107: ldfld LandPlotUI/PlotPurchaseItem EmptyPlotUI::silo
IL_010c: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_0111: ldc.i4 4003
IL_0116: newobj System.Void System.Nullable`1<PediaDirector/Id>::.ctor(!0)
IL_011b: ldarg.0
IL_011c: ldftn System.Void EmptyPlotUI::BuySilo()
IL_0122: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_0127: ldc.i4.1
IL_0128: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_012d: stelem.ref
IL_012e: dup
IL_012f: ldc.i4.4
IL_0130: ldstr "t.incinerator"
IL_0135: ldarg.0
IL_0136: ldfld LandPlotUI/PlotPurchaseItem EmptyPlotUI::incinerator
IL_013b: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_0140: ldarg.0
IL_0141: ldfld LandPlotUI/PlotPurchaseItem EmptyPlotUI::incinerator
IL_0146: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_014b: ldstr "m.intro.incinerator"
IL_0150: ldarg.0
IL_0151: ldfld LandPlotUI/PlotPurchaseItem EmptyPlotUI::incinerator
IL_0156: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_015b: ldc.i4 4004
IL_0160: newobj System.Void System.Nullable`1<PediaDirector/Id>::.ctor(!0)
IL_0165: ldarg.0
IL_0166: ldftn System.Void EmptyPlotUI::BuyIncinerator()
IL_016c: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_0171: ldc.i4.1
IL_0172: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_0177: stelem.ref
IL_0178: dup
IL_0179: ldc.i4.5
IL_017a: ldstr "t.pond"
IL_017f: ldarg.0
IL_0180: ldfld LandPlotUI/PlotPurchaseItem EmptyPlotUI::pond
IL_0185: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_018a: ldarg.0
IL_018b: ldfld LandPlotUI/PlotPurchaseItem EmptyPlotUI::pond
IL_0190: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_0195: ldstr "m.intro.pond"
IL_019a: ldarg.0
IL_019b: ldfld LandPlotUI/PlotPurchaseItem EmptyPlotUI::pond
IL_01a0: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_01a5: ldc.i4 4005
IL_01aa: newobj System.Void System.Nullable`1<PediaDirector/Id>::.ctor(!0)
IL_01af: ldarg.0
IL_01b0: ldftn System.Void EmptyPlotUI::BuyPond()
IL_01b6: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_01bb: ldc.i4.1
IL_01bc: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_01c1: stelem.ref
IL_01c2: stloc.0
IL_01c3: call T SRSingleton`1<GameContext>::get_Instance()
IL_01c8: callvirt UITemplates GameContext::get_UITemplates()
IL_01cd: ldarg.0
IL_01ce: ldfld UnityEngine.Sprite EmptyPlotUI::titleIcon
IL_01d3: ldstr "ui"
IL_01d8: ldstr "t.empty_plot"
IL_01dd: call System.String MessageUtil::Qualify(System.String,System.String)
IL_01e2: ldloc.0
IL_01e3: ldarg.0
IL_01e4: dup
IL_01e5: ldvirtftn System.Void BaseUI::Close()
IL_01eb: newobj System.Void PurchaseUI/OnClose::.ctor(System.Object,System.IntPtr)
IL_01f0: callvirt UnityEngine.GameObject UITemplates::CreatePurchaseUI(UnityEngine.Sprite,System.String,PurchaseUI/Purchasable[],PurchaseUI/OnClose)
IL_01f5: ret

*/
//  GardenUI.CreatePurchaseUI
/*
IL_0000: ldc.i4.5
IL_0001: newarr PurchaseUI/Purchasable
IL_0006: dup
IL_0007: ldc.i4.0
IL_0008: ldstr "m.upgrade.name.garden.soil"
IL_000d: ldarg.0
IL_000e: ldfld LandPlotUI/UpgradePurchaseItem GardenUI::soil
IL_0013: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_0018: ldarg.0
IL_0019: ldfld LandPlotUI/UpgradePurchaseItem GardenUI::soil
IL_001e: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_0023: ldstr "m.upgrade.desc.garden.soil"
IL_0028: ldarg.0
IL_0029: ldfld LandPlotUI/UpgradePurchaseItem GardenUI::soil
IL_002e: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_0033: ldc.i4 4002
IL_0038: newobj System.Void System.Nullable`1<PediaDirector/Id>::.ctor(!0)
IL_003d: ldarg.0
IL_003e: ldftn System.Void GardenUI::UpgradeSoil()
IL_0044: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_0049: ldarg.0
IL_004a: ldfld LandPlot LandPlotUI::activator
IL_004f: ldc.i4.6
IL_0050: callvirt System.Boolean LandPlot::HasUpgrade(LandPlot/Upgrade)
IL_0055: ldc.i4.0
IL_0056: ceq
IL_0058: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_005d: stelem.ref
IL_005e: dup
IL_005f: ldc.i4.1
IL_0060: ldstr "m.upgrade.name.garden.sprinkler"
IL_0065: ldarg.0
IL_0066: ldfld LandPlotUI/UpgradePurchaseItem GardenUI::sprinkler
IL_006b: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_0070: ldarg.0
IL_0071: ldfld LandPlotUI/UpgradePurchaseItem GardenUI::sprinkler
IL_0076: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_007b: ldstr "m.upgrade.desc.garden.sprinkler"
IL_0080: ldarg.0
IL_0081: ldfld LandPlotUI/UpgradePurchaseItem GardenUI::sprinkler
IL_0086: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_008b: ldc.i4 4002
IL_0090: newobj System.Void System.Nullable`1<PediaDirector/Id>::.ctor(!0)
IL_0095: ldarg.0
IL_0096: ldftn System.Void GardenUI::UpgradeSprinkler()
IL_009c: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_00a1: ldarg.0
IL_00a2: ldfld LandPlot LandPlotUI::activator
IL_00a7: ldc.i4.7
IL_00a8: callvirt System.Boolean LandPlot::HasUpgrade(LandPlot/Upgrade)
IL_00ad: ldc.i4.0
IL_00ae: ceq
IL_00b0: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_00b5: stelem.ref
IL_00b6: dup
IL_00b7: ldc.i4.2
IL_00b8: ldstr "m.upgrade.name.garden.scareslime"
IL_00bd: ldarg.0
IL_00be: ldfld LandPlotUI/UpgradePurchaseItem GardenUI::scareslime
IL_00c3: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_00c8: ldarg.0
IL_00c9: ldfld LandPlotUI/UpgradePurchaseItem GardenUI::scareslime
IL_00ce: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_00d3: ldstr "m.upgrade.desc.garden.scareslime"
IL_00d8: ldarg.0
IL_00d9: ldfld LandPlotUI/UpgradePurchaseItem GardenUI::scareslime
IL_00de: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_00e3: ldc.i4 4002
IL_00e8: newobj System.Void System.Nullable`1<PediaDirector/Id>::.ctor(!0)
IL_00ed: ldarg.0
IL_00ee: ldftn System.Void GardenUI::UpgradeScareslime()
IL_00f4: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_00f9: ldarg.0
IL_00fa: ldfld LandPlot LandPlotUI::activator
IL_00ff: ldc.i4.8
IL_0100: callvirt System.Boolean LandPlot::HasUpgrade(LandPlot/Upgrade)
IL_0105: ldc.i4.0
IL_0106: ceq
IL_0108: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_010d: stelem.ref
IL_010e: dup
IL_010f: ldc.i4.3
IL_0110: ldstr "ui"
IL_0115: ldstr "b.clear_crop"
IL_011a: call System.String MessageUtil::Qualify(System.String,System.String)
IL_011f: ldarg.0
IL_0120: ldfld LandPlotUI/PurchaseItem GardenUI::clearCrop
IL_0125: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_012a: ldarg.0
IL_012b: ldfld LandPlotUI/PurchaseItem GardenUI::clearCrop
IL_0130: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_0135: ldstr "ui"
IL_013a: ldstr "m.desc.clear_crop"
IL_013f: call System.String MessageUtil::Qualify(System.String,System.String)
IL_0144: ldarg.0
IL_0145: ldfld LandPlotUI/PurchaseItem GardenUI::clearCrop
IL_014a: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_014f: ldloca.s V_1
IL_0151: initobj System.Nullable`1<PediaDirector/Id>
IL_0157: ldloc.1
IL_0158: ldarg.0
IL_0159: ldftn System.Void GardenUI::ClearCrop()
IL_015f: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_0164: ldarg.0
IL_0165: ldfld LandPlot LandPlotUI::activator
IL_016a: callvirt System.Boolean LandPlot::HasAttached()
IL_016f: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_0174: stelem.ref
IL_0175: dup
IL_0176: ldc.i4.4
IL_0177: ldstr "ui"
IL_017c: ldstr "b.demolish"
IL_0181: call System.String MessageUtil::Qualify(System.String,System.String)
IL_0186: ldarg.0
IL_0187: ldfld LandPlotUI/PlotPurchaseItem GardenUI::demolish
IL_018c: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_0191: ldarg.0
IL_0192: ldfld LandPlotUI/PlotPurchaseItem GardenUI::demolish
IL_0197: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_019c: ldstr "ui"
IL_01a1: ldstr "m.desc.demolish"
IL_01a6: call System.String MessageUtil::Qualify(System.String,System.String)
IL_01ab: ldarg.0
IL_01ac: ldfld LandPlotUI/PlotPurchaseItem GardenUI::demolish
IL_01b1: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_01b6: ldloca.s V_2
IL_01b8: initobj System.Nullable`1<PediaDirector/Id>
IL_01be: ldloc.2
IL_01bf: ldarg.0
IL_01c0: ldftn System.Void GardenUI::Demolish()
IL_01c6: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_01cb: ldc.i4.1
IL_01cc: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_01d1: stelem.ref
IL_01d2: stloc.0
IL_01d3: call T SRSingleton`1<GameContext>::get_Instance()
IL_01d8: callvirt UITemplates GameContext::get_UITemplates()
IL_01dd: ldarg.0
IL_01de: ldfld UnityEngine.Sprite GardenUI::titleIcon
IL_01e3: ldstr "t.garden"
IL_01e8: ldloc.0
IL_01e9: ldarg.0
IL_01ea: dup
IL_01eb: ldvirtftn System.Void BaseUI::Close()
IL_01f1: newobj System.Void PurchaseUI/OnClose::.ctor(System.Object,System.IntPtr)
IL_01f6: callvirt UnityEngine.GameObject UITemplates::CreatePurchaseUI(UnityEngine.Sprite,System.String,PurchaseUI/Purchasable[],PurchaseUI/OnClose)
IL_01fb: ret

*/
//  CorralUI.CreatePurchaseUI
/*
IL_0000: ldc.i4.7
IL_0001: newarr PurchaseUI/Purchasable
IL_0006: dup
IL_0007: ldc.i4.0
IL_0008: ldstr "m.upgrade.name.corral.walls"
IL_000d: ldarg.0
IL_000e: ldfld LandPlotUI/UpgradePurchaseItem CorralUI::walls
IL_0013: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_0018: ldarg.0
IL_0019: ldfld LandPlotUI/UpgradePurchaseItem CorralUI::walls
IL_001e: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_0023: ldstr "m.upgrade.desc.corral.walls"
IL_0028: ldarg.0
IL_0029: ldfld LandPlotUI/UpgradePurchaseItem CorralUI::walls
IL_002e: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_0033: ldc.i4 4000
IL_0038: newobj System.Void System.Nullable`1<PediaDirector/Id>::.ctor(!0)
IL_003d: ldarg.0
IL_003e: ldftn System.Void CorralUI::UpgradeWalls()
IL_0044: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_0049: ldarg.0
IL_004a: ldfld LandPlot LandPlotUI::activator
IL_004f: ldc.i4.1
IL_0050: callvirt System.Boolean LandPlot::HasUpgrade(LandPlot/Upgrade)
IL_0055: ldc.i4.0
IL_0056: ceq
IL_0058: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_005d: stelem.ref
IL_005e: dup
IL_005f: ldc.i4.1
IL_0060: ldstr "m.upgrade.name.corral.music_box"
IL_0065: ldarg.0
IL_0066: ldfld LandPlotUI/UpgradePurchaseItem CorralUI::musicBox
IL_006b: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_0070: ldarg.0
IL_0071: ldfld LandPlotUI/UpgradePurchaseItem CorralUI::musicBox
IL_0076: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_007b: ldstr "m.upgrade.desc.corral.music_box"
IL_0080: ldarg.0
IL_0081: ldfld LandPlotUI/UpgradePurchaseItem CorralUI::musicBox
IL_0086: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_008b: ldc.i4 4000
IL_0090: newobj System.Void System.Nullable`1<PediaDirector/Id>::.ctor(!0)
IL_0095: ldarg.0
IL_0096: ldftn System.Void CorralUI::UpgradeMusicBox()
IL_009c: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_00a1: ldarg.0
IL_00a2: ldfld LandPlot LandPlotUI::activator
IL_00a7: ldc.i4.2
IL_00a8: callvirt System.Boolean LandPlot::HasUpgrade(LandPlot/Upgrade)
IL_00ad: ldc.i4.0
IL_00ae: ceq
IL_00b0: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_00b5: stelem.ref
IL_00b6: dup
IL_00b7: ldc.i4.2
IL_00b8: ldstr "m.upgrade.name.corral.air_net"
IL_00bd: ldarg.0
IL_00be: ldfld LandPlotUI/UpgradePurchaseItem CorralUI::airNet
IL_00c3: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_00c8: ldarg.0
IL_00c9: ldfld LandPlotUI/UpgradePurchaseItem CorralUI::airNet
IL_00ce: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_00d3: ldstr "m.upgrade.desc.corral.air_net"
IL_00d8: ldarg.0
IL_00d9: ldfld LandPlotUI/UpgradePurchaseItem CorralUI::airNet
IL_00de: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_00e3: ldc.i4 4000
IL_00e8: newobj System.Void System.Nullable`1<PediaDirector/Id>::.ctor(!0)
IL_00ed: ldarg.0
IL_00ee: ldftn System.Void CorralUI::UpgradeAirNet()
IL_00f4: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_00f9: ldarg.0
IL_00fa: ldfld LandPlot LandPlotUI::activator
IL_00ff: ldc.i4.s 11
IL_0101: callvirt System.Boolean LandPlot::HasUpgrade(LandPlot/Upgrade)
IL_0106: ldc.i4.0
IL_0107: ceq
IL_0109: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_010e: stelem.ref
IL_010f: dup
IL_0110: ldc.i4.3
IL_0111: ldstr "m.upgrade.name.corral.solar_shield"
IL_0116: ldarg.0
IL_0117: ldfld LandPlotUI/UpgradePurchaseItem CorralUI::solarShield
IL_011c: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_0121: ldarg.0
IL_0122: ldfld LandPlotUI/UpgradePurchaseItem CorralUI::solarShield
IL_0127: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_012c: ldstr "m.upgrade.desc.corral.solar_shield"
IL_0131: ldarg.0
IL_0132: ldfld LandPlotUI/UpgradePurchaseItem CorralUI::solarShield
IL_0137: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_013c: ldc.i4 4000
IL_0141: newobj System.Void System.Nullable`1<PediaDirector/Id>::.ctor(!0)
IL_0146: ldarg.0
IL_0147: ldftn System.Void CorralUI::UpgradeSolarShield()
IL_014d: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_0152: ldarg.0
IL_0153: ldfld LandPlot LandPlotUI::activator
IL_0158: ldc.i4.s 13
IL_015a: callvirt System.Boolean LandPlot::HasUpgrade(LandPlot/Upgrade)
IL_015f: ldc.i4.0
IL_0160: ceq
IL_0162: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_0167: stelem.ref
IL_0168: dup
IL_0169: ldc.i4.4
IL_016a: ldstr "m.upgrade.name.corral.plort_collector"
IL_016f: ldarg.0
IL_0170: ldfld LandPlotUI/UpgradePurchaseItem CorralUI::plortCollector
IL_0175: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_017a: ldarg.0
IL_017b: ldfld LandPlotUI/UpgradePurchaseItem CorralUI::plortCollector
IL_0180: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_0185: ldstr "m.upgrade.desc.corral.plort_collector"
IL_018a: ldarg.0
IL_018b: ldfld LandPlotUI/UpgradePurchaseItem CorralUI::plortCollector
IL_0190: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_0195: ldc.i4 4000
IL_019a: newobj System.Void System.Nullable`1<PediaDirector/Id>::.ctor(!0)
IL_019f: ldarg.0
IL_01a0: ldftn System.Void CorralUI::UpgradePlortCollector()
IL_01a6: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_01ab: ldarg.0
IL_01ac: ldfld LandPlot LandPlotUI::activator
IL_01b1: ldc.i4.s 12
IL_01b3: callvirt System.Boolean LandPlot::HasUpgrade(LandPlot/Upgrade)
IL_01b8: ldc.i4.0
IL_01b9: ceq
IL_01bb: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_01c0: stelem.ref
IL_01c1: dup
IL_01c2: ldc.i4.5
IL_01c3: ldstr "m.upgrade.name.corral.feeder"
IL_01c8: ldarg.0
IL_01c9: ldfld LandPlotUI/UpgradePurchaseItem CorralUI::feeder
IL_01ce: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_01d3: ldarg.0
IL_01d4: ldfld LandPlotUI/UpgradePurchaseItem CorralUI::feeder
IL_01d9: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_01de: ldstr "m.upgrade.desc.corral.feeder"
IL_01e3: ldarg.0
IL_01e4: ldfld LandPlotUI/UpgradePurchaseItem CorralUI::feeder
IL_01e9: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_01ee: ldc.i4 4000
IL_01f3: newobj System.Void System.Nullable`1<PediaDirector/Id>::.ctor(!0)
IL_01f8: ldarg.0
IL_01f9: ldftn System.Void CorralUI::UpgradeFeeder()
IL_01ff: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_0204: ldarg.0
IL_0205: ldfld LandPlot LandPlotUI::activator
IL_020a: ldc.i4.s 9
IL_020c: callvirt System.Boolean LandPlot::HasUpgrade(LandPlot/Upgrade)
IL_0211: ldc.i4.0
IL_0212: ceq
IL_0214: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_0219: stelem.ref
IL_021a: dup
IL_021b: ldc.i4.6
IL_021c: ldstr "ui"
IL_0221: ldstr "b.demolish"
IL_0226: call System.String MessageUtil::Qualify(System.String,System.String)
IL_022b: ldarg.0
IL_022c: ldfld LandPlotUI/PlotPurchaseItem CorralUI::demolish
IL_0231: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_0236: ldarg.0
IL_0237: ldfld LandPlotUI/PlotPurchaseItem CorralUI::demolish
IL_023c: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_0241: ldstr "ui"
IL_0246: ldstr "m.desc.demolish"
IL_024b: call System.String MessageUtil::Qualify(System.String,System.String)
IL_0250: ldarg.0
IL_0251: ldfld LandPlotUI/PlotPurchaseItem CorralUI::demolish
IL_0256: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_025b: ldloca.s V_1
IL_025d: initobj System.Nullable`1<PediaDirector/Id>
IL_0263: ldloc.1
IL_0264: ldarg.0
IL_0265: ldftn System.Void CorralUI::Demolish()
IL_026b: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_0270: ldc.i4.1
IL_0271: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_0276: stelem.ref
IL_0277: stloc.0
IL_0278: call T SRSingleton`1<GameContext>::get_Instance()
IL_027d: callvirt UITemplates GameContext::get_UITemplates()
IL_0282: ldarg.0
IL_0283: ldfld UnityEngine.Sprite CorralUI::titleIcon
IL_0288: ldstr "t.corral"
IL_028d: ldloc.0
IL_028e: ldarg.0
IL_028f: dup
IL_0290: ldvirtftn System.Void BaseUI::Close()
IL_0296: newobj System.Void PurchaseUI/OnClose::.ctor(System.Object,System.IntPtr)
IL_029b: callvirt UnityEngine.GameObject UITemplates::CreatePurchaseUI(UnityEngine.Sprite,System.String,PurchaseUI/Purchasable[],PurchaseUI/OnClose)
IL_02a0: ret

*/
//  CoopUI.CreatePurchaseUI
/*
IL_0000: ldc.i4.4
IL_0001: newarr PurchaseUI/Purchasable
IL_0006: dup
IL_0007: ldc.i4.0
IL_0008: ldstr "m.upgrade.name.coop.walls"
IL_000d: ldarg.0
IL_000e: ldfld LandPlotUI/UpgradePurchaseItem CoopUI::walls
IL_0013: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_0018: ldarg.0
IL_0019: ldfld LandPlotUI/UpgradePurchaseItem CoopUI::walls
IL_001e: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_0023: ldstr "m.upgrade.desc.coop.walls"
IL_0028: ldarg.0
IL_0029: ldfld LandPlotUI/UpgradePurchaseItem CoopUI::walls
IL_002e: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_0033: ldc.i4 4001
IL_0038: newobj System.Void System.Nullable`1<PediaDirector/Id>::.ctor(!0)
IL_003d: ldarg.0
IL_003e: ldftn System.Void CoopUI::UpgradeWalls()
IL_0044: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_0049: ldarg.0
IL_004a: ldfld LandPlot LandPlotUI::activator
IL_004f: ldc.i4.1
IL_0050: callvirt System.Boolean LandPlot::HasUpgrade(LandPlot/Upgrade)
IL_0055: ldc.i4.0
IL_0056: ceq
IL_0058: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_005d: stelem.ref
IL_005e: dup
IL_005f: ldc.i4.1
IL_0060: ldstr "m.upgrade.name.coop.feeder"
IL_0065: ldarg.0
IL_0066: ldfld LandPlotUI/UpgradePurchaseItem CoopUI::feeder
IL_006b: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_0070: ldarg.0
IL_0071: ldfld LandPlotUI/UpgradePurchaseItem CoopUI::feeder
IL_0076: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_007b: ldstr "m.upgrade.desc.coop.feeder"
IL_0080: ldarg.0
IL_0081: ldfld LandPlotUI/UpgradePurchaseItem CoopUI::feeder
IL_0086: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_008b: ldc.i4 4001
IL_0090: newobj System.Void System.Nullable`1<PediaDirector/Id>::.ctor(!0)
IL_0095: ldarg.0
IL_0096: ldftn System.Void CoopUI::UpgradeFeeder()
IL_009c: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_00a1: ldarg.0
IL_00a2: ldfld LandPlot LandPlotUI::activator
IL_00a7: ldc.i4.s 9
IL_00a9: callvirt System.Boolean LandPlot::HasUpgrade(LandPlot/Upgrade)
IL_00ae: ldc.i4.0
IL_00af: ceq
IL_00b1: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_00b6: stelem.ref
IL_00b7: dup
IL_00b8: ldc.i4.2
IL_00b9: ldstr "m.upgrade.name.coop.vitamizer"
IL_00be: ldarg.0
IL_00bf: ldfld LandPlotUI/UpgradePurchaseItem CoopUI::vitamizer
IL_00c4: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_00c9: ldarg.0
IL_00ca: ldfld LandPlotUI/UpgradePurchaseItem CoopUI::vitamizer
IL_00cf: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_00d4: ldstr "m.upgrade.desc.coop.vitamizer"
IL_00d9: ldarg.0
IL_00da: ldfld LandPlotUI/UpgradePurchaseItem CoopUI::vitamizer
IL_00df: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_00e4: ldc.i4 4001
IL_00e9: newobj System.Void System.Nullable`1<PediaDirector/Id>::.ctor(!0)
IL_00ee: ldarg.0
IL_00ef: ldftn System.Void CoopUI::UpgradeVitamizer()
IL_00f5: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_00fa: ldarg.0
IL_00fb: ldfld LandPlot LandPlotUI::activator
IL_0100: ldc.i4.s 10
IL_0102: callvirt System.Boolean LandPlot::HasUpgrade(LandPlot/Upgrade)
IL_0107: ldc.i4.0
IL_0108: ceq
IL_010a: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_010f: stelem.ref
IL_0110: dup
IL_0111: ldc.i4.3
IL_0112: ldstr "ui"
IL_0117: ldstr "b.demolish"
IL_011c: call System.String MessageUtil::Qualify(System.String,System.String)
IL_0121: ldarg.0
IL_0122: ldfld LandPlotUI/PlotPurchaseItem CoopUI::demolish
IL_0127: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_012c: ldarg.0
IL_012d: ldfld LandPlotUI/PlotPurchaseItem CoopUI::demolish
IL_0132: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_0137: ldstr "ui"
IL_013c: ldstr "m.desc.demolish"
IL_0141: call System.String MessageUtil::Qualify(System.String,System.String)
IL_0146: ldarg.0
IL_0147: ldfld LandPlotUI/PlotPurchaseItem CoopUI::demolish
IL_014c: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_0151: ldloca.s V_1
IL_0153: initobj System.Nullable`1<PediaDirector/Id>
IL_0159: ldloc.1
IL_015a: ldarg.0
IL_015b: ldftn System.Void CoopUI::Demolish()
IL_0161: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_0166: ldc.i4.1
IL_0167: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_016c: stelem.ref
IL_016d: stloc.0
IL_016e: call T SRSingleton`1<GameContext>::get_Instance()
IL_0173: callvirt UITemplates GameContext::get_UITemplates()
IL_0178: ldarg.0
IL_0179: ldfld UnityEngine.Sprite CoopUI::titleIcon
IL_017e: ldstr "t.coop"
IL_0183: ldloc.0
IL_0184: ldarg.0
IL_0185: dup
IL_0186: ldvirtftn System.Void BaseUI::Close()
IL_018c: newobj System.Void PurchaseUI/OnClose::.ctor(System.Object,System.IntPtr)
IL_0191: callvirt UnityEngine.GameObject UITemplates::CreatePurchaseUI(UnityEngine.Sprite,System.String,PurchaseUI/Purchasable[],PurchaseUI/OnClose)
IL_0196: ret

*/
//  PondUI.CreatePurchaseUI
/*
IL_0000: ldc.i4.1
IL_0001: newarr PurchaseUI/Purchasable
IL_0006: dup
IL_0007: ldc.i4.0
IL_0008: ldstr "ui"
IL_000d: ldstr "b.demolish"
IL_0012: call System.String MessageUtil::Qualify(System.String,System.String)
IL_0017: ldarg.0
IL_0018: ldfld LandPlotUI/PlotPurchaseItem PondUI::demolish
IL_001d: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_0022: ldarg.0
IL_0023: ldfld LandPlotUI/PlotPurchaseItem PondUI::demolish
IL_0028: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_002d: ldstr "ui"
IL_0032: ldstr "m.desc.demolish"
IL_0037: call System.String MessageUtil::Qualify(System.String,System.String)
IL_003c: ldarg.0
IL_003d: ldfld LandPlotUI/PlotPurchaseItem PondUI::demolish
IL_0042: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_0047: ldloca.s V_1
IL_0049: initobj System.Nullable`1<PediaDirector/Id>
IL_004f: ldloc.1
IL_0050: ldarg.0
IL_0051: ldftn System.Void PondUI::Demolish()
IL_0057: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_005c: ldc.i4.1
IL_005d: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_0062: stelem.ref
IL_0063: stloc.0
IL_0064: call T SRSingleton`1<GameContext>::get_Instance()
IL_0069: callvirt UITemplates GameContext::get_UITemplates()
IL_006e: ldarg.0
IL_006f: ldfld UnityEngine.Sprite PondUI::titleIcon
IL_0074: ldstr "t.pond"
IL_0079: ldloc.0
IL_007a: ldarg.0
IL_007b: dup
IL_007c: ldvirtftn System.Void BaseUI::Close()
IL_0082: newobj System.Void PurchaseUI/OnClose::.ctor(System.Object,System.IntPtr)
IL_0087: callvirt UnityEngine.GameObject UITemplates::CreatePurchaseUI(UnityEngine.Sprite,System.String,PurchaseUI/Purchasable[],PurchaseUI/OnClose)
IL_008c: ret

*/
//  SiloUI.CreatePurchaseUI
/*
IL_0000: ldc.i4.4
IL_0001: newarr PurchaseUI/Purchasable
IL_0006: dup
IL_0007: ldc.i4.0
IL_0008: ldstr "m.upgrade.name.silo.storage2"
IL_000d: ldarg.0
IL_000e: ldfld LandPlotUI/UpgradePurchaseItem SiloUI::storage2
IL_0013: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_0018: ldarg.0
IL_0019: ldfld LandPlotUI/UpgradePurchaseItem SiloUI::storage2
IL_001e: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_0023: ldstr "m.upgrade.desc.silo.storage2"
IL_0028: ldarg.0
IL_0029: ldfld LandPlotUI/UpgradePurchaseItem SiloUI::storage2
IL_002e: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_0033: ldc.i4 4003
IL_0038: newobj System.Void System.Nullable`1<PediaDirector/Id>::.ctor(!0)
IL_003d: ldarg.0
IL_003e: ldftn System.Void SiloUI::UpgradeStorage2()
IL_0044: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_0049: ldarg.0
IL_004a: ldfld LandPlot LandPlotUI::activator
IL_004f: ldc.i4.3
IL_0050: callvirt System.Boolean LandPlot::HasUpgrade(LandPlot/Upgrade)
IL_0055: ldc.i4.0
IL_0056: ceq
IL_0058: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_005d: stelem.ref
IL_005e: dup
IL_005f: ldc.i4.1
IL_0060: ldstr "m.upgrade.name.silo.storage2"
IL_0065: ldarg.0
IL_0066: ldfld LandPlotUI/UpgradePurchaseItem SiloUI::storage3
IL_006b: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_0070: ldarg.0
IL_0071: ldfld LandPlotUI/UpgradePurchaseItem SiloUI::storage3
IL_0076: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_007b: ldstr "m.upgrade.desc.silo.storage2"
IL_0080: ldarg.0
IL_0081: ldfld LandPlotUI/UpgradePurchaseItem SiloUI::storage3
IL_0086: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_008b: ldc.i4 4003
IL_0090: newobj System.Void System.Nullable`1<PediaDirector/Id>::.ctor(!0)
IL_0095: ldarg.0
IL_0096: ldftn System.Void SiloUI::UpgradeStorage3()
IL_009c: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_00a1: ldarg.0
IL_00a2: ldfld LandPlot LandPlotUI::activator
IL_00a7: ldc.i4.4
IL_00a8: callvirt System.Boolean LandPlot::HasUpgrade(LandPlot/Upgrade)
IL_00ad: brtrue IL_00c0
IL_00b2: ldarg.0
IL_00b3: ldfld LandPlot LandPlotUI::activator
IL_00b8: ldc.i4.3
IL_00b9: callvirt System.Boolean LandPlot::HasUpgrade(LandPlot/Upgrade)
IL_00be: br.s IL_00c1
IL_00c0: ldc.i4.0
IL_00c1: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_00c6: stelem.ref
IL_00c7: dup
IL_00c8: ldc.i4.2
IL_00c9: ldstr "m.upgrade.name.silo.storage2"
IL_00ce: ldarg.0
IL_00cf: ldfld LandPlotUI/UpgradePurchaseItem SiloUI::storage4
IL_00d4: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_00d9: ldarg.0
IL_00da: ldfld LandPlotUI/UpgradePurchaseItem SiloUI::storage4
IL_00df: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_00e4: ldstr "m.upgrade.desc.silo.storage2"
IL_00e9: ldarg.0
IL_00ea: ldfld LandPlotUI/UpgradePurchaseItem SiloUI::storage4
IL_00ef: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_00f4: ldc.i4 4003
IL_00f9: newobj System.Void System.Nullable`1<PediaDirector/Id>::.ctor(!0)
IL_00fe: ldarg.0
IL_00ff: ldftn System.Void SiloUI::UpgradeStorage4()
IL_0105: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_010a: ldarg.0
IL_010b: ldfld LandPlot LandPlotUI::activator
IL_0110: ldc.i4.5
IL_0111: callvirt System.Boolean LandPlot::HasUpgrade(LandPlot/Upgrade)
IL_0116: brtrue IL_0129
IL_011b: ldarg.0
IL_011c: ldfld LandPlot LandPlotUI::activator
IL_0121: ldc.i4.4
IL_0122: callvirt System.Boolean LandPlot::HasUpgrade(LandPlot/Upgrade)
IL_0127: br.s IL_012a
IL_0129: ldc.i4.0
IL_012a: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_012f: stelem.ref
IL_0130: dup
IL_0131: ldc.i4.3
IL_0132: ldstr "ui"
IL_0137: ldstr "b.demolish"
IL_013c: call System.String MessageUtil::Qualify(System.String,System.String)
IL_0141: ldarg.0
IL_0142: ldfld LandPlotUI/PlotPurchaseItem SiloUI::demolish
IL_0147: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_014c: ldarg.0
IL_014d: ldfld LandPlotUI/PlotPurchaseItem SiloUI::demolish
IL_0152: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_0157: ldstr "ui"
IL_015c: ldstr "m.desc.demolish"
IL_0161: call System.String MessageUtil::Qualify(System.String,System.String)
IL_0166: ldarg.0
IL_0167: ldfld LandPlotUI/PlotPurchaseItem SiloUI::demolish
IL_016c: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_0171: ldloca.s V_1
IL_0173: initobj System.Nullable`1<PediaDirector/Id>
IL_0179: ldloc.1
IL_017a: ldarg.0
IL_017b: ldftn System.Void SiloUI::Demolish()
IL_0181: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_0186: ldc.i4.1
IL_0187: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_018c: stelem.ref
IL_018d: stloc.0
IL_018e: call T SRSingleton`1<GameContext>::get_Instance()
IL_0193: callvirt UITemplates GameContext::get_UITemplates()
IL_0198: ldarg.0
IL_0199: ldfld UnityEngine.Sprite SiloUI::titleIcon
IL_019e: ldstr "t.silo"
IL_01a3: ldloc.0
IL_01a4: ldarg.0
IL_01a5: dup
IL_01a6: ldvirtftn System.Void BaseUI::Close()
IL_01ac: newobj System.Void PurchaseUI/OnClose::.ctor(System.Object,System.IntPtr)
IL_01b1: callvirt UnityEngine.GameObject UITemplates::CreatePurchaseUI(UnityEngine.Sprite,System.String,PurchaseUI/Purchasable[],PurchaseUI/OnClose)
IL_01b6: ret

*/
//  IncineratorUI.CreatePurchaseUI
/*
IL_0000: ldc.i4.1
IL_0001: newarr PurchaseUI/Purchasable
IL_0006: dup
IL_0007: ldc.i4.0
IL_0008: ldstr "ui"
IL_000d: ldstr "b.demolish"
IL_0012: call System.String MessageUtil::Qualify(System.String,System.String)
IL_0017: ldarg.0
IL_0018: ldfld LandPlotUI/PlotPurchaseItem IncineratorUI::demolish
IL_001d: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::icon
IL_0022: ldarg.0
IL_0023: ldfld LandPlotUI/PlotPurchaseItem IncineratorUI::demolish
IL_0028: ldfld UnityEngine.Sprite LandPlotUI/PurchaseItem::img
IL_002d: ldstr "ui"
IL_0032: ldstr "m.desc.demolish"
IL_0037: call System.String MessageUtil::Qualify(System.String,System.String)
IL_003c: ldarg.0
IL_003d: ldfld LandPlotUI/PlotPurchaseItem IncineratorUI::demolish
IL_0042: ldfld System.Int32 LandPlotUI/PurchaseItem::cost
IL_0047: ldloca.s V_1
IL_0049: initobj System.Nullable`1<PediaDirector/Id>
IL_004f: ldloc.1
IL_0050: ldarg.0
IL_0051: ldftn System.Void IncineratorUI::Demolish()
IL_0057: newobj System.Void UnityEngine.Events.UnityAction::.ctor(System.Object,System.IntPtr)
IL_005c: ldc.i4.1
IL_005d: newobj System.Void PurchaseUI/Purchasable::.ctor(System.String,UnityEngine.Sprite,UnityEngine.Sprite,System.String,System.Int32,System.Nullable`1<PediaDirector/Id>,UnityEngine.Events.UnityAction,System.Boolean)
IL_0062: stelem.ref
IL_0063: stloc.0
IL_0064: call T SRSingleton`1<GameContext>::get_Instance()
IL_0069: callvirt UITemplates GameContext::get_UITemplates()
IL_006e: ldarg.0
IL_006f: ldfld UnityEngine.Sprite IncineratorUI::titleIcon
IL_0074: ldstr "t.incinerator"
IL_0079: ldloc.0
IL_007a: ldarg.0
IL_007b: dup
IL_007c: ldvirtftn System.Void BaseUI::Close()
IL_0082: newobj System.Void PurchaseUI/OnClose::.ctor(System.Object,System.IntPtr)
IL_0087: callvirt UnityEngine.GameObject UITemplates::CreatePurchaseUI(UnityEngine.Sprite,System.String,PurchaseUI/Purchasable[],PurchaseUI/OnClose)
IL_008c: ret

*/
//  EconomyDirector.InitForLevel
/*
IL_0000: ldarg.0
IL_0001: call T SRSingleton`1<SceneContext>::get_Instance()
IL_0006: callvirt TimeDirector SceneContext::get_TimeDirector()
IL_000b: stfld TimeDirector EconomyDirector::timeDir
IL_0010: ldarg.0
IL_0011: call T SRSingleton`1<SceneContext>::get_Instance()
IL_0016: callvirt PlayerState SceneContext::get_PlayerState()
IL_001b: stfld PlayerState EconomyDirector::playerState
IL_0020: ldarg.0
IL_0021: ldc.r4 0
IL_0026: stfld System.Single EconomyDirector::nextUpdateTime
IL_002b: ldarg.0
IL_002c: ldfld System.Single EconomyDirector::saturationRecovery
IL_0031: ldc.r4 0
IL_0036: blt IL_004b
IL_003b: ldarg.0
IL_003c: ldfld System.Single EconomyDirector::saturationRecovery
IL_0041: ldc.r4 1
IL_0046: ble.un IL_0056
IL_004b: ldstr "Saturation Recovery must be [0-1]"
IL_0050: newobj System.Void System.ArgumentException::.ctor(System.String)
IL_0055: throw
IL_0056: ldarg.0
IL_0057: newobj System.Void Randoms::.ctor()
IL_005c: ldc.r4 1000000
IL_0061: callvirt System.Single Randoms::GetFloat(System.Single)
IL_0066: stfld System.Single EconomyDirector::noiseSeed
IL_006b: ldarg.0
IL_006c: ldfld EconomyDirector/ValueMap[] EconomyDirector::baseValueMap
IL_0071: stloc.1
IL_0072: ldc.i4.0
IL_0073: stloc.2
IL_0074: br IL_00d6
IL_0079: ldloc.1
IL_007a: ldloc.2
IL_007b: ldelem.ref
IL_007c: stloc.0
IL_007d: ldarg.0
IL_007e: ldfld System.Collections.Generic.Dictionary`2<Identifiable/Id,EconomyDirector/CurrValueEntry> EconomyDirector::currValueMap
IL_0083: ldloc.0
IL_0084: ldfld Identifiable EconomyDirector/ValueMap::accept
IL_0089: ldfld Identifiable/Id Identifiable::id
IL_008e: ldloc.0
IL_008f: ldfld System.Single EconomyDirector/ValueMap::value
IL_0094: ldloc.0
IL_0095: ldfld System.Single EconomyDirector/ValueMap::value
IL_009a: ldloc.0
IL_009b: ldfld System.Single EconomyDirector/ValueMap::value
IL_00a0: ldloc.0
IL_00a1: ldfld System.Single EconomyDirector/ValueMap::fullSaturation
IL_00a6: newobj System.Void EconomyDirector/CurrValueEntry::.ctor(System.Single,System.Single,System.Single,System.Single)
IL_00ab: callvirt System.Void System.Collections.Generic.Dictionary`2<Identifiable/Id,EconomyDirector/CurrValueEntry>::set_Item(!0,!1)
IL_00b0: ldarg.0
IL_00b1: ldfld System.Collections.Generic.Dictionary`2<Identifiable/Id,System.Single> EconomyDirector::marketSaturation
IL_00b6: ldloc.0
IL_00b7: ldfld Identifiable EconomyDirector/ValueMap::accept
IL_00bc: ldfld Identifiable/Id Identifiable::id
IL_00c1: ldloc.0
IL_00c2: ldfld System.Single EconomyDirector/ValueMap::fullSaturation
IL_00c7: ldc.r4 0.5
IL_00cc: mul
IL_00cd: callvirt System.Void System.Collections.Generic.Dictionary`2<Identifiable/Id,System.Single>::set_Item(!0,!1)
IL_00d2: ldloc.2
IL_00d3: ldc.i4.1
IL_00d4: add
IL_00d5: stloc.2
IL_00d6: ldloc.2
IL_00d7: ldloc.1
IL_00d8: ldlen
IL_00d9: conv.i4
IL_00da: blt IL_0079
IL_00df: ret

*/
//  EconomyDirector.InitForLevel
/*
IL_0000: ldarg.0
IL_0001: call T SRSingleton`1<SceneContext>::get_Instance()
IL_0006: callvirt TimeDirector SceneContext::get_TimeDirector()
IL_000b: stfld TimeDirector EconomyDirector::timeDir
IL_0010: ldarg.0
IL_0011: call T SRSingleton`1<SceneContext>::get_Instance()
IL_0016: callvirt PlayerState SceneContext::get_PlayerState()
IL_001b: stfld PlayerState EconomyDirector::playerState
IL_0020: ldarg.0
IL_0021: ldc.r4 0
IL_0026: stfld System.Single EconomyDirector::nextUpdateTime
IL_002b: ldarg.0
IL_002c: ldfld System.Single EconomyDirector::saturationRecovery
IL_0031: ldc.r4 0
IL_0036: blt IL_004b
IL_003b: ldarg.0
IL_003c: ldfld System.Single EconomyDirector::saturationRecovery
IL_0041: ldc.r4 1
IL_0046: ble.un IL_0056
IL_004b: ldstr "Saturation Recovery must be [0-1]"
IL_0050: newobj System.Void System.ArgumentException::.ctor(System.String)
IL_0055: throw
IL_0056: ldarg.0
IL_0057: newobj System.Void Randoms::.ctor()
IL_005c: ldc.r4 1000000
IL_0061: callvirt System.Single Randoms::GetFloat(System.Single)
IL_0066: stfld System.Single EconomyDirector::noiseSeed
IL_006b: ldarg.0
IL_006c: ldfld EconomyDirector/ValueMap[] EconomyDirector::baseValueMap
IL_0071: stloc.1
IL_0072: ldc.i4.0
IL_0073: stloc.2
IL_0074: br IL_00d6
IL_0079: ldloc.1
IL_007a: ldloc.2
IL_007b: ldelem.ref
IL_007c: stloc.0
IL_007d: ldarg.0
IL_007e: ldfld System.Collections.Generic.Dictionary`2<Identifiable/Id,EconomyDirector/CurrValueEntry> EconomyDirector::currValueMap
IL_0083: ldloc.0
IL_0084: ldfld Identifiable EconomyDirector/ValueMap::accept
IL_0089: ldfld Identifiable/Id Identifiable::id
IL_008e: ldloc.0
IL_008f: ldfld System.Single EconomyDirector/ValueMap::value
IL_0094: ldloc.0
IL_0095: ldfld System.Single EconomyDirector/ValueMap::value
IL_009a: ldloc.0
IL_009b: ldfld System.Single EconomyDirector/ValueMap::value
IL_00a0: ldloc.0
IL_00a1: ldfld System.Single EconomyDirector/ValueMap::fullSaturation
IL_00a6: newobj System.Void EconomyDirector/CurrValueEntry::.ctor(System.Single,System.Single,System.Single,System.Single)
IL_00ab: callvirt System.Void System.Collections.Generic.Dictionary`2<Identifiable/Id,EconomyDirector/CurrValueEntry>::set_Item(!0,!1)
IL_00b0: ldarg.0
IL_00b1: ldfld System.Collections.Generic.Dictionary`2<Identifiable/Id,System.Single> EconomyDirector::marketSaturation
IL_00b6: ldloc.0
IL_00b7: ldfld Identifiable EconomyDirector/ValueMap::accept
IL_00bc: ldfld Identifiable/Id Identifiable::id
IL_00c1: ldloc.0
IL_00c2: ldfld System.Single EconomyDirector/ValueMap::fullSaturation
IL_00c7: ldc.r4 0.5
IL_00cc: mul
IL_00cd: callvirt System.Void System.Collections.Generic.Dictionary`2<Identifiable/Id,System.Single>::set_Item(!0,!1)
IL_00d2: ldloc.2
IL_00d3: ldc.i4.1
IL_00d4: add
IL_00d5: stloc.2
IL_00d6: ldloc.2
IL_00d7: ldloc.1
IL_00d8: ldlen
IL_00d9: conv.i4
IL_00da: blt IL_0079
IL_00df: ret

*/
//  EconomyDirector.Update
/*
IL_0000: ldarg.0
IL_0001: ldfld TimeDirector EconomyDirector::timeDir
IL_0006: ldarg.0
IL_0007: ldfld System.Single EconomyDirector::nextUpdateTime
IL_000c: callvirt System.Boolean TimeDirector::HasReached(System.Single)
IL_0011: brfalse IL_0113
IL_0016: ldarg.0
IL_0017: ldfld TimeDirector EconomyDirector::timeDir
IL_001c: callvirt System.Int32 TimeDirector::CurrDay()
IL_0021: stloc.0
IL_0022: ldarg.0
IL_0023: ldfld System.Collections.Generic.Dictionary`2<Identifiable/Id,EconomyDirector/CurrValueEntry> EconomyDirector::currValueMap
IL_0028: callvirt System.Collections.Generic.Dictionary`2/Enumerator<!0,!1> System.Collections.Generic.Dictionary`2<Identifiable/Id,EconomyDirector/CurrValueEntry>::GetEnumerator()
IL_002d: stloc.2
IL_002e: br IL_00ca
IL_0033: ldloca.s V_2
IL_0035: call System.Collections.Generic.KeyValuePair`2<!0,!1> System.Collections.Generic.Dictionary`2/Enumerator<Identifiable/Id,EconomyDirector/CurrValueEntry>::get_Current()
IL_003a: stloc.1
IL_003b: ldarg.0
IL_003c: ldfld System.Single EconomyDirector::nextUpdateTime
IL_0041: ldc.r4 0
IL_0046: ble.un IL_007d
IL_004b: ldarg.0
IL_004c: ldfld System.Collections.Generic.Dictionary`2<Identifiable/Id,System.Single> EconomyDirector::marketSaturation
IL_0051: dup
IL_0052: stloc.s V_4
IL_0054: ldloca.s V_1
IL_0056: call !0 System.Collections.Generic.KeyValuePair`2<Identifiable/Id,EconomyDirector/CurrValueEntry>::get_Key()
IL_005b: dup
IL_005c: stloc.s V_5
IL_005e: ldloc.s V_4
IL_0060: ldloc.s V_5
IL_0062: callvirt !1 System.Collections.Generic.Dictionary`2<Identifiable/Id,System.Single>::get_Item(!0)
IL_0067: stloc.s V_6
IL_0069: ldloc.s V_6
IL_006b: ldc.r4 1
IL_0070: ldarg.0
IL_0071: ldfld System.Single EconomyDirector::saturationRecovery
IL_0076: sub
IL_0077: mul
IL_0078: callvirt System.Void System.Collections.Generic.Dictionary`2<Identifiable/Id,System.Single>::set_Item(!0,!1)
IL_007d: ldarg.0
IL_007e: ldloca.s V_1
IL_0080: call !0 System.Collections.Generic.KeyValuePair`2<Identifiable/Id,EconomyDirector/CurrValueEntry>::get_Key()
IL_0085: ldloca.s V_1
IL_0087: call !1 System.Collections.Generic.KeyValuePair`2<Identifiable/Id,EconomyDirector/CurrValueEntry>::get_Value()
IL_008c: ldfld System.Single EconomyDirector/CurrValueEntry::baseValue
IL_0091: ldloca.s V_1
IL_0093: call !1 System.Collections.Generic.KeyValuePair`2<Identifiable/Id,EconomyDirector/CurrValueEntry>::get_Value()
IL_0098: ldfld System.Single EconomyDirector/CurrValueEntry::fullSaturation
IL_009d: ldloc.0
IL_009e: conv.r4
IL_009f: call System.Single EconomyDirector::GetTargetValue(Identifiable/Id,System.Single,System.Single,System.Single)
IL_00a4: stloc.3
IL_00a5: ldloca.s V_1
IL_00a7: call !1 System.Collections.Generic.KeyValuePair`2<Identifiable/Id,EconomyDirector/CurrValueEntry>::get_Value()
IL_00ac: ldloca.s V_1
IL_00ae: call !1 System.Collections.Generic.KeyValuePair`2<Identifiable/Id,EconomyDirector/CurrValueEntry>::get_Value()
IL_00b3: ldfld System.Single EconomyDirector/CurrValueEntry::currValue
IL_00b8: stfld System.Single EconomyDirector/CurrValueEntry::prevValue
IL_00bd: ldloca.s V_1
IL_00bf: call !1 System.Collections.Generic.KeyValuePair`2<Identifiable/Id,EconomyDirector/CurrValueEntry>::get_Value()
IL_00c4: ldloc.3
IL_00c5: stfld System.Single EconomyDirector/CurrValueEntry::currValue
IL_00ca: ldloca.s V_2
IL_00cc: call System.Boolean System.Collections.Generic.Dictionary`2/Enumerator<Identifiable/Id,EconomyDirector/CurrValueEntry>::MoveNext()
IL_00d1: brtrue IL_0033
IL_00d6: leave IL_00e7
IL_00db: ldloc.2
IL_00dc: box System.Collections.Generic.Dictionary`2/Enumerator<Identifiable/Id,EconomyDirector/CurrValueEntry>
IL_00e1: callvirt System.Void System.IDisposable::Dispose()
IL_00e6: endfinally
IL_00e7: ldarg.0
IL_00e8: ldarg.0
IL_00e9: ldfld TimeDirector EconomyDirector::timeDir
IL_00ee: ldc.r4 0
IL_00f3: callvirt System.Single TimeDirector::GetNextHour(System.Single)
IL_00f8: stfld System.Single EconomyDirector::nextUpdateTime
IL_00fd: ldarg.0
IL_00fe: ldfld EconomyDirector/DidUpdate EconomyDirector::didUpdateDelegate
IL_0103: brfalse IL_0113
IL_0108: ldarg.0
IL_0109: ldfld EconomyDirector/DidUpdate EconomyDirector::didUpdateDelegate
IL_010e: callvirt System.Void EconomyDirector/DidUpdate::Invoke()
IL_0113: ret

*/
//  SiloCatcher.OnTriggerEnter
/*
IL_0000: ldarg.1
IL_0001: callvirt System.Boolean UnityEngine.Collider::get_isTrigger()
IL_0006: brtrue IL_00e0
IL_000b: ldarg.0
IL_000c: ldfld System.Boolean SiloCatcher::allowsInput
IL_0011: brfalse IL_00e0
IL_0016: ldarg.1
IL_0017: callvirt UnityEngine.GameObject UnityEngine.Component::get_gameObject()
IL_001c: stloc.0
IL_001d: ldloc.0
IL_001e: callvirt !!0 UnityEngine.GameObject::GetComponent<Identifiable>()
IL_0023: stloc.1
IL_0024: ldloc.0
IL_0025: callvirt !!0 UnityEngine.GameObject::GetComponent<Vacuumable>()
IL_002a: stloc.2
IL_002b: ldloc.1
IL_002c: ldnull
IL_002d: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_0032: brfalse IL_00e0
IL_0037: ldarg.0
IL_0038: ldfld System.Collections.Generic.HashSet`1<UnityEngine.GameObject> SiloCatcher::collectedThisFrame
IL_003d: ldloc.0
IL_003e: callvirt System.Boolean System.Collections.Generic.HashSet`1<UnityEngine.GameObject>::Contains(!0)
IL_0043: brtrue IL_00e0
IL_0048: ldloc.2
IL_0049: ldnull
IL_004a: call System.Boolean UnityEngine.Object::op_Equality(UnityEngine.Object,UnityEngine.Object)
IL_004f: brtrue IL_005f
IL_0054: ldloc.2
IL_0055: callvirt System.Boolean Vacuumable::isCaptive()
IL_005a: brtrue IL_00e0
IL_005f: ldarg.0
IL_0060: ldfld SiloStorage SiloCatcher::storage
IL_0065: ldloc.1
IL_0066: ldarg.0
IL_0067: ldfld System.Int32 SiloCatcher::slotIdx
IL_006c: callvirt System.Boolean SiloStorage::MaybeAddIdentifiable(Identifiable,System.Int32)
IL_0071: brfalse IL_00e0
IL_0076: ldarg.0
IL_0077: ldfld UnityEngine.GameObject SiloCatcher::storeFX
IL_007c: ldloc.0
IL_007d: callvirt UnityEngine.Transform UnityEngine.GameObject::get_transform()
IL_0082: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_position()
IL_0087: ldloc.0
IL_0088: callvirt UnityEngine.Transform UnityEngine.GameObject::get_transform()
IL_008d: callvirt UnityEngine.Quaternion UnityEngine.Transform::get_rotation()
IL_0092: call UnityEngine.GameObject SRBehaviour::InstantiateDynamic(UnityEngine.GameObject,UnityEngine.Vector3,UnityEngine.Quaternion)
IL_0097: pop
IL_0098: ldarg.0
IL_0099: ldfld System.Collections.Generic.HashSet`1<UnityEngine.GameObject> SiloCatcher::collectedThisFrame
IL_009e: ldloc.0
IL_009f: callvirt System.Boolean System.Collections.Generic.HashSet`1<UnityEngine.GameObject>::Add(!0)
IL_00a4: pop
IL_00a5: ldloc.0
IL_00a6: callvirt !!0 UnityEngine.GameObject::GetComponent<DestroyOnTouching>()
IL_00ab: stloc.3
IL_00ac: ldloc.3
IL_00ad: ldnull
IL_00ae: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_00b3: brfalse IL_00be
IL_00b8: ldloc.3
IL_00b9: callvirt System.Void DestroyOnTouching::NoteDestroying()
IL_00be: ldloc.0
IL_00bf: call System.Void UnityEngine.Object::Destroy(UnityEngine.Object)
IL_00c4: ldarg.0
IL_00c5: ldfld SECTR_AudioSource SiloCatcher::scoreAudio
IL_00ca: callvirt System.Void SECTR_AudioSource::Play()
IL_00cf: ldarg.0
IL_00d0: call System.Single UnityEngine.Time::get_time()
IL_00d5: ldc.r4 1
IL_00da: add
IL_00db: stfld System.Single SiloCatcher::accelInUntil
IL_00e0: ret

*/
//  SiloCatcher.OnTriggerEnter
/*
IL_0000: ldarg.1
IL_0001: callvirt System.Boolean UnityEngine.Collider::get_isTrigger()
IL_0006: brtrue IL_00e0
IL_000b: ldarg.0
IL_000c: ldfld System.Boolean SiloCatcher::allowsInput
IL_0011: brfalse IL_00e0
IL_0016: ldarg.1
IL_0017: callvirt UnityEngine.GameObject UnityEngine.Component::get_gameObject()
IL_001c: stloc.0
IL_001d: ldloc.0
IL_001e: callvirt !!0 UnityEngine.GameObject::GetComponent<Identifiable>()
IL_0023: stloc.1
IL_0024: ldloc.0
IL_0025: callvirt !!0 UnityEngine.GameObject::GetComponent<Vacuumable>()
IL_002a: stloc.2
IL_002b: ldloc.1
IL_002c: ldnull
IL_002d: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_0032: brfalse IL_00e0
IL_0037: ldarg.0
IL_0038: ldfld System.Collections.Generic.HashSet`1<UnityEngine.GameObject> SiloCatcher::collectedThisFrame
IL_003d: ldloc.0
IL_003e: callvirt System.Boolean System.Collections.Generic.HashSet`1<UnityEngine.GameObject>::Contains(!0)
IL_0043: brtrue IL_00e0
IL_0048: ldloc.2
IL_0049: ldnull
IL_004a: call System.Boolean UnityEngine.Object::op_Equality(UnityEngine.Object,UnityEngine.Object)
IL_004f: brtrue IL_005f
IL_0054: ldloc.2
IL_0055: callvirt System.Boolean Vacuumable::isCaptive()
IL_005a: brtrue IL_00e0
IL_005f: ldarg.0
IL_0060: ldfld SiloStorage SiloCatcher::storage
IL_0065: ldloc.1
IL_0066: ldarg.0
IL_0067: ldfld System.Int32 SiloCatcher::slotIdx
IL_006c: callvirt System.Boolean SiloStorage::MaybeAddIdentifiable(Identifiable,System.Int32)
IL_0071: brfalse IL_00e0
IL_0076: ldarg.0
IL_0077: ldfld UnityEngine.GameObject SiloCatcher::storeFX
IL_007c: ldloc.0
IL_007d: callvirt UnityEngine.Transform UnityEngine.GameObject::get_transform()
IL_0082: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_position()
IL_0087: ldloc.0
IL_0088: callvirt UnityEngine.Transform UnityEngine.GameObject::get_transform()
IL_008d: callvirt UnityEngine.Quaternion UnityEngine.Transform::get_rotation()
IL_0092: call UnityEngine.GameObject SRBehaviour::InstantiateDynamic(UnityEngine.GameObject,UnityEngine.Vector3,UnityEngine.Quaternion)
IL_0097: pop
IL_0098: ldarg.0
IL_0099: ldfld System.Collections.Generic.HashSet`1<UnityEngine.GameObject> SiloCatcher::collectedThisFrame
IL_009e: ldloc.0
IL_009f: callvirt System.Boolean System.Collections.Generic.HashSet`1<UnityEngine.GameObject>::Add(!0)
IL_00a4: pop
IL_00a5: ldloc.0
IL_00a6: callvirt !!0 UnityEngine.GameObject::GetComponent<DestroyOnTouching>()
IL_00ab: stloc.3
IL_00ac: ldloc.3
IL_00ad: ldnull
IL_00ae: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_00b3: brfalse IL_00be
IL_00b8: ldloc.3
IL_00b9: callvirt System.Void DestroyOnTouching::NoteDestroying()
IL_00be: ldloc.0
IL_00bf: call System.Void UnityEngine.Object::Destroy(UnityEngine.Object)
IL_00c4: ldarg.0
IL_00c5: ldfld SECTR_AudioSource SiloCatcher::scoreAudio
IL_00ca: callvirt System.Void SECTR_AudioSource::Play()
IL_00cf: ldarg.0
IL_00d0: call System.Single UnityEngine.Time::get_time()
IL_00d5: ldc.r4 1
IL_00da: add
IL_00db: stfld System.Single SiloCatcher::accelInUntil
IL_00e0: ret

*/
//  SiloCatcher.OnTriggerStay
/*
IL_0000: ldarg.0
IL_0001: ldfld System.Boolean SiloCatcher::allowsOutput
IL_0006: brfalse IL_0149
IL_000b: ldarg.1
IL_000c: callvirt UnityEngine.GameObject UnityEngine.Component::get_gameObject()
IL_0011: callvirt !!0 UnityEngine.GameObject::GetComponentInParent<SiloActivator>()
IL_0016: stloc.0
IL_0017: ldloc.0
IL_0018: ldnull
IL_0019: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_001e: brfalse IL_0149
IL_0023: ldloc.0
IL_0024: callvirt System.Boolean UnityEngine.Behaviour::get_enabled()
IL_0029: brfalse IL_0149
IL_002e: call System.Single UnityEngine.Time::get_time()
IL_0033: ldarg.0
IL_0034: ldfld System.Single SiloCatcher::nextEject
IL_0039: ble.un IL_0149
IL_003e: ldarg.0
IL_003f: ldfld SiloStorage SiloCatcher::storage
IL_0044: callvirt Ammo SiloStorage::get_Ammo()
IL_0049: ldarg.0
IL_004a: ldfld System.Int32 SiloCatcher::slotIdx
IL_004f: callvirt System.Boolean Ammo::SetAmmoSlot(System.Int32)
IL_0054: pop
IL_0055: ldarg.0
IL_0056: ldfld SiloStorage SiloCatcher::storage
IL_005b: callvirt Ammo SiloStorage::get_Ammo()
IL_0060: callvirt System.Boolean Ammo::HasSelectedAmmo()
IL_0065: brfalse IL_0149
IL_006a: ldarg.1
IL_006b: callvirt UnityEngine.GameObject UnityEngine.Component::get_gameObject()
IL_0070: callvirt UnityEngine.Transform UnityEngine.GameObject::get_transform()
IL_0075: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_position()
IL_007a: ldarg.0
IL_007b: call UnityEngine.Transform UnityEngine.Component::get_transform()
IL_0080: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_position()
IL_0085: call UnityEngine.Vector3 UnityEngine.Vector3::op_Subtraction(UnityEngine.Vector3,UnityEngine.Vector3)
IL_008a: stloc.s V_4
IL_008c: ldloca.s V_4
IL_008e: call UnityEngine.Vector3 UnityEngine.Vector3::get_normalized()
IL_0093: stloc.1
IL_0094: ldarg.0
IL_0095: call UnityEngine.Transform UnityEngine.Component::get_transform()
IL_009a: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_forward()
IL_009f: ldloc.1
IL_00a0: call System.Single UnityEngine.Vector3::Angle(UnityEngine.Vector3,UnityEngine.Vector3)
IL_00a5: call System.Single UnityEngine.Mathf::Abs(System.Single)
IL_00aa: ldc.r4 45
IL_00af: bgt.un IL_0149
IL_00b4: ldarg.0
IL_00b5: ldfld SiloStorage SiloCatcher::storage
IL_00ba: callvirt Ammo SiloStorage::get_Ammo()
IL_00bf: callvirt UnityEngine.GameObject Ammo::GetSelectedStored()
IL_00c4: ldarg.0
IL_00c5: call UnityEngine.Transform UnityEngine.Component::get_transform()
IL_00ca: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_position()
IL_00cf: ldloc.1
IL_00d0: ldc.r4 1.2
IL_00d5: call UnityEngine.Vector3 UnityEngine.Vector3::op_Multiply(UnityEngine.Vector3,System.Single)
IL_00da: call UnityEngine.Vector3 UnityEngine.Vector3::op_Addition(UnityEngine.Vector3,UnityEngine.Vector3)
IL_00df: ldarg.0
IL_00e0: call UnityEngine.Transform UnityEngine.Component::get_transform()
IL_00e5: callvirt UnityEngine.Quaternion UnityEngine.Transform::get_rotation()
IL_00ea: call UnityEngine.GameObject SRBehaviour::InstantiateDynamic(UnityEngine.GameObject,UnityEngine.Vector3,UnityEngine.Quaternion)
IL_00ef: stloc.2
IL_00f0: ldarg.0
IL_00f1: ldfld SiloStorage SiloCatcher::storage
IL_00f6: callvirt Ammo SiloStorage::get_Ammo()
IL_00fb: ldc.i4.1
IL_00fc: callvirt System.Void Ammo::DecrementSelectedAmmo(System.Int32)
IL_0101: ldloc.2
IL_0102: callvirt !!0 UnityEngine.GameObject::GetComponent<Vacuumable>()
IL_0107: stloc.3
IL_0108: ldloc.3
IL_0109: ldnull
IL_010a: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_010f: brfalse IL_0120
IL_0114: ldarg.0
IL_0115: ldfld WeaponVacuum SiloCatcher::vac
IL_011a: ldloc.3
IL_011b: callvirt System.Void WeaponVacuum::ForceJoint(Vacuumable)
IL_0120: ldarg.0
IL_0121: call System.Single UnityEngine.Time::get_time()
IL_0126: ldc.r4 0.25
IL_012b: ldarg.0
IL_012c: ldfld System.Single SiloCatcher::outSpeedFactor
IL_0131: div
IL_0132: add
IL_0133: stfld System.Single SiloCatcher::nextEject
IL_0138: ldarg.0
IL_0139: call System.Single UnityEngine.Time::get_time()
IL_013e: ldc.r4 1
IL_0143: add
IL_0144: stfld System.Single SiloCatcher::accelOutUntil
IL_0149: ret

*/
//  SiloCatcher.OnTriggerStay
/*
IL_0000: ldarg.0
IL_0001: ldfld System.Boolean SiloCatcher::allowsOutput
IL_0006: brfalse IL_0149
IL_000b: ldarg.1
IL_000c: callvirt UnityEngine.GameObject UnityEngine.Component::get_gameObject()
IL_0011: callvirt !!0 UnityEngine.GameObject::GetComponentInParent<SiloActivator>()
IL_0016: stloc.0
IL_0017: ldloc.0
IL_0018: ldnull
IL_0019: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_001e: brfalse IL_0149
IL_0023: ldloc.0
IL_0024: callvirt System.Boolean UnityEngine.Behaviour::get_enabled()
IL_0029: brfalse IL_0149
IL_002e: call System.Single UnityEngine.Time::get_time()
IL_0033: ldarg.0
IL_0034: ldfld System.Single SiloCatcher::nextEject
IL_0039: ble.un IL_0149
IL_003e: ldarg.0
IL_003f: ldfld SiloStorage SiloCatcher::storage
IL_0044: callvirt Ammo SiloStorage::get_Ammo()
IL_0049: ldarg.0
IL_004a: ldfld System.Int32 SiloCatcher::slotIdx
IL_004f: callvirt System.Boolean Ammo::SetAmmoSlot(System.Int32)
IL_0054: pop
IL_0055: ldarg.0
IL_0056: ldfld SiloStorage SiloCatcher::storage
IL_005b: callvirt Ammo SiloStorage::get_Ammo()
IL_0060: callvirt System.Boolean Ammo::HasSelectedAmmo()
IL_0065: brfalse IL_0149
IL_006a: ldarg.1
IL_006b: callvirt UnityEngine.GameObject UnityEngine.Component::get_gameObject()
IL_0070: callvirt UnityEngine.Transform UnityEngine.GameObject::get_transform()
IL_0075: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_position()
IL_007a: ldarg.0
IL_007b: call UnityEngine.Transform UnityEngine.Component::get_transform()
IL_0080: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_position()
IL_0085: call UnityEngine.Vector3 UnityEngine.Vector3::op_Subtraction(UnityEngine.Vector3,UnityEngine.Vector3)
IL_008a: stloc.s V_4
IL_008c: ldloca.s V_4
IL_008e: call UnityEngine.Vector3 UnityEngine.Vector3::get_normalized()
IL_0093: stloc.1
IL_0094: ldarg.0
IL_0095: call UnityEngine.Transform UnityEngine.Component::get_transform()
IL_009a: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_forward()
IL_009f: ldloc.1
IL_00a0: call System.Single UnityEngine.Vector3::Angle(UnityEngine.Vector3,UnityEngine.Vector3)
IL_00a5: call System.Single UnityEngine.Mathf::Abs(System.Single)
IL_00aa: ldc.r4 45
IL_00af: bgt.un IL_0149
IL_00b4: ldarg.0
IL_00b5: ldfld SiloStorage SiloCatcher::storage
IL_00ba: callvirt Ammo SiloStorage::get_Ammo()
IL_00bf: callvirt UnityEngine.GameObject Ammo::GetSelectedStored()
IL_00c4: ldarg.0
IL_00c5: call UnityEngine.Transform UnityEngine.Component::get_transform()
IL_00ca: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_position()
IL_00cf: ldloc.1
IL_00d0: ldc.r4 1.2
IL_00d5: call UnityEngine.Vector3 UnityEngine.Vector3::op_Multiply(UnityEngine.Vector3,System.Single)
IL_00da: call UnityEngine.Vector3 UnityEngine.Vector3::op_Addition(UnityEngine.Vector3,UnityEngine.Vector3)
IL_00df: ldarg.0
IL_00e0: call UnityEngine.Transform UnityEngine.Component::get_transform()
IL_00e5: callvirt UnityEngine.Quaternion UnityEngine.Transform::get_rotation()
IL_00ea: call UnityEngine.GameObject SRBehaviour::InstantiateDynamic(UnityEngine.GameObject,UnityEngine.Vector3,UnityEngine.Quaternion)
IL_00ef: stloc.2
IL_00f0: ldarg.0
IL_00f1: ldfld SiloStorage SiloCatcher::storage
IL_00f6: callvirt Ammo SiloStorage::get_Ammo()
IL_00fb: ldc.i4.1
IL_00fc: callvirt System.Void Ammo::DecrementSelectedAmmo(System.Int32)
IL_0101: ldloc.2
IL_0102: callvirt !!0 UnityEngine.GameObject::GetComponent<Vacuumable>()
IL_0107: stloc.3
IL_0108: ldloc.3
IL_0109: ldnull
IL_010a: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_010f: brfalse IL_0120
IL_0114: ldarg.0
IL_0115: ldfld WeaponVacuum SiloCatcher::vac
IL_011a: ldloc.3
IL_011b: callvirt System.Void WeaponVacuum::ForceJoint(Vacuumable)
IL_0120: ldarg.0
IL_0121: call System.Single UnityEngine.Time::get_time()
IL_0126: ldc.r4 0.25
IL_012b: ldarg.0
IL_012c: ldfld System.Single SiloCatcher::outSpeedFactor
IL_0131: div
IL_0132: add
IL_0133: stfld System.Single SiloCatcher::nextEject
IL_0138: ldarg.0
IL_0139: call System.Single UnityEngine.Time::get_time()
IL_013e: ldc.r4 1
IL_0143: add
IL_0144: stfld System.Single SiloCatcher::accelOutUntil
IL_0149: ret

*/
//  SpawnResource.Start
/*
IL_0000: ldarg.0
IL_0001: ldarg.0
IL_0002: call !!0 UnityEngine.Component::GetComponentInParent<LandPlot>()
IL_0007: stfld LandPlot SpawnResource::landPlot
IL_000c: ldarg.0
IL_000d: ldfld TimeDirector SpawnResource::timeDir
IL_0012: callvirt System.Boolean TimeDirector::IsAtStart()
IL_0017: brfalse IL_0028
IL_001c: ldarg.0
IL_001d: ldc.i4.1
IL_001e: call System.Void SpawnResource::Spawn(System.Boolean)
IL_0023: br IL_0049
IL_0028: ldarg.0
IL_0029: ldfld System.Single SpawnResource::nextSpawnTime
IL_002e: ldc.r4 0
IL_0033: bne.un IL_0049
IL_0038: ldarg.0
IL_0039: ldarg.0
IL_003a: ldfld TimeDirector SpawnResource::timeDir
IL_003f: callvirt System.Single TimeDirector::WorldTime()
IL_0044: stfld System.Single SpawnResource::nextSpawnTime
IL_0049: ret

*/
//  GardenCatcher.Awake
/*
IL_0000: ldarg.0
IL_0001: ldfld GardenCatcher/PlantSlot[] GardenCatcher::plantable
IL_0006: stloc.1
IL_0007: ldc.i4.0
IL_0008: stloc.2
IL_0009: br IL_002d
IL_000e: ldloc.1
IL_000f: ldloc.2
IL_0010: ldelem.ref
IL_0011: stloc.0
IL_0012: ldarg.0
IL_0013: ldfld System.Collections.Generic.Dictionary`2<Identifiable/Id,UnityEngine.GameObject> GardenCatcher::plantableDict
IL_0018: ldloc.0
IL_0019: ldfld Identifiable/Id GardenCatcher/PlantSlot::id
IL_001e: ldloc.0
IL_001f: ldfld UnityEngine.GameObject GardenCatcher/PlantSlot::plantedPrefab
IL_0024: callvirt System.Void System.Collections.Generic.Dictionary`2<Identifiable/Id,UnityEngine.GameObject>::set_Item(!0,!1)
IL_0029: ldloc.2
IL_002a: ldc.i4.1
IL_002b: add
IL_002c: stloc.2
IL_002d: ldloc.2
IL_002e: ldloc.1
IL_002f: ldlen
IL_0030: conv.i4
IL_0031: blt IL_000e
IL_0036: ldarg.0
IL_0037: call T SRSingleton`1<SceneContext>::get_Instance()
IL_003c: callvirt TutorialDirector SceneContext::get_TutorialDirector()
IL_0041: stfld TutorialDirector GardenCatcher::tutDir
IL_0046: ret

*/
//  GardenCatcher.Awake
/*
IL_0000: ldarg.0
IL_0001: ldfld GardenCatcher/PlantSlot[] GardenCatcher::plantable
IL_0006: stloc.1
IL_0007: ldc.i4.0
IL_0008: stloc.2
IL_0009: br IL_002d
IL_000e: ldloc.1
IL_000f: ldloc.2
IL_0010: ldelem.ref
IL_0011: stloc.0
IL_0012: ldarg.0
IL_0013: ldfld System.Collections.Generic.Dictionary`2<Identifiable/Id,UnityEngine.GameObject> GardenCatcher::plantableDict
IL_0018: ldloc.0
IL_0019: ldfld Identifiable/Id GardenCatcher/PlantSlot::id
IL_001e: ldloc.0
IL_001f: ldfld UnityEngine.GameObject GardenCatcher/PlantSlot::plantedPrefab
IL_0024: callvirt System.Void System.Collections.Generic.Dictionary`2<Identifiable/Id,UnityEngine.GameObject>::set_Item(!0,!1)
IL_0029: ldloc.2
IL_002a: ldc.i4.1
IL_002b: add
IL_002c: stloc.2
IL_002d: ldloc.2
IL_002e: ldloc.1
IL_002f: ldlen
IL_0030: conv.i4
IL_0031: blt IL_000e
IL_0036: ldarg.0
IL_0037: call T SRSingleton`1<SceneContext>::get_Instance()
IL_003c: callvirt TutorialDirector SceneContext::get_TutorialDirector()
IL_0041: stfld TutorialDirector GardenCatcher::tutDir
IL_0046: ret

*/
//  GardenCatcher.OnTriggerEnter
/*
IL_0000: ldarg.1
IL_0001: callvirt System.Boolean UnityEngine.Collider::get_isTrigger()
IL_0006: brfalse IL_000c
IL_000b: ret
IL_000c: ldarg.0
IL_000d: ldfld LandPlot GardenCatcher::activator
IL_0012: callvirt System.Boolean LandPlot::HasAttached()
IL_0017: brfalse IL_001d
IL_001c: ret
IL_001d: ldarg.1
IL_001e: callvirt !!0 UnityEngine.Component::GetComponent<Identifiable>()
IL_0023: stloc.0
IL_0024: ldloc.0
IL_0025: ldnull
IL_0026: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_002b: brfalse IL_00de
IL_0030: ldarg.0
IL_0031: ldfld System.Collections.Generic.Dictionary`2<Identifiable/Id,UnityEngine.GameObject> GardenCatcher::plantableDict
IL_0036: ldloc.0
IL_0037: ldfld Identifiable/Id Identifiable::id
IL_003c: callvirt System.Boolean System.Collections.Generic.Dictionary`2<Identifiable/Id,UnityEngine.GameObject>::ContainsKey(!0)
IL_0041: brfalse IL_00de
IL_0046: ldarg.0
IL_0047: ldfld System.Collections.Generic.Dictionary`2<Identifiable/Id,UnityEngine.GameObject> GardenCatcher::plantableDict
IL_004c: ldloc.0
IL_004d: ldfld Identifiable/Id Identifiable::id
IL_0052: callvirt !1 System.Collections.Generic.Dictionary`2<Identifiable/Id,UnityEngine.GameObject>::get_Item(!0)
IL_0057: ldarg.0
IL_0058: ldfld LandPlot GardenCatcher::activator
IL_005d: callvirt UnityEngine.Transform UnityEngine.Component::get_transform()
IL_0062: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_position()
IL_0067: ldarg.0
IL_0068: ldfld LandPlot GardenCatcher::activator
IL_006d: callvirt UnityEngine.Transform UnityEngine.Component::get_transform()
IL_0072: callvirt UnityEngine.Quaternion UnityEngine.Transform::get_rotation()
IL_0077: call UnityEngine.Object UnityEngine.Object::Instantiate(UnityEngine.Object,UnityEngine.Vector3,UnityEngine.Quaternion)
IL_007c: isinst UnityEngine.GameObject
IL_0081: stloc.1
IL_0082: ldloc.1
IL_0083: callvirt !!0 UnityEngine.GameObject::AddComponent<DestroyAfterTime>()
IL_0088: pop
IL_0089: ldarg.0
IL_008a: ldfld LandPlot GardenCatcher::activator
IL_008f: ldloc.1
IL_0090: callvirt System.Void LandPlot::Attach(UnityEngine.GameObject)
IL_0095: ldarg.0
IL_0096: ldfld TutorialDirector GardenCatcher::tutDir
IL_009b: callvirt System.Void TutorialDirector::OnPlanted()
IL_00a0: ldarg.0
IL_00a1: ldfld UnityEngine.GameObject GardenCatcher::acceptFX
IL_00a6: ldnull
IL_00a7: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_00ac: brfalse IL_00d3
IL_00b1: ldarg.0
IL_00b2: ldfld UnityEngine.GameObject GardenCatcher::acceptFX
IL_00b7: ldarg.0
IL_00b8: call UnityEngine.Transform UnityEngine.Component::get_transform()
IL_00bd: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_position()
IL_00c2: ldarg.0
IL_00c3: call UnityEngine.Transform UnityEngine.Component::get_transform()
IL_00c8: callvirt UnityEngine.Quaternion UnityEngine.Transform::get_rotation()
IL_00cd: call UnityEngine.GameObject SRBehaviour::InstantiateDynamic(UnityEngine.GameObject,UnityEngine.Vector3,UnityEngine.Quaternion)
IL_00d2: pop
IL_00d3: ldarg.1
IL_00d4: callvirt UnityEngine.GameObject UnityEngine.Component::get_gameObject()
IL_00d9: call System.Void UnityEngine.Object::Destroy(UnityEngine.Object)
IL_00de: ret

*/
//  GardenCatcher.OnTriggerEnter
/*
IL_0000: ldarg.1
IL_0001: callvirt System.Boolean UnityEngine.Collider::get_isTrigger()
IL_0006: brfalse IL_000c
IL_000b: ret
IL_000c: ldarg.0
IL_000d: ldfld LandPlot GardenCatcher::activator
IL_0012: callvirt System.Boolean LandPlot::HasAttached()
IL_0017: brfalse IL_001d
IL_001c: ret
IL_001d: ldarg.1
IL_001e: callvirt !!0 UnityEngine.Component::GetComponent<Identifiable>()
IL_0023: stloc.0
IL_0024: ldloc.0
IL_0025: ldnull
IL_0026: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_002b: brfalse IL_00de
IL_0030: ldarg.0
IL_0031: ldfld System.Collections.Generic.Dictionary`2<Identifiable/Id,UnityEngine.GameObject> GardenCatcher::plantableDict
IL_0036: ldloc.0
IL_0037: ldfld Identifiable/Id Identifiable::id
IL_003c: callvirt System.Boolean System.Collections.Generic.Dictionary`2<Identifiable/Id,UnityEngine.GameObject>::ContainsKey(!0)
IL_0041: brfalse IL_00de
IL_0046: ldarg.0
IL_0047: ldfld System.Collections.Generic.Dictionary`2<Identifiable/Id,UnityEngine.GameObject> GardenCatcher::plantableDict
IL_004c: ldloc.0
IL_004d: ldfld Identifiable/Id Identifiable::id
IL_0052: callvirt !1 System.Collections.Generic.Dictionary`2<Identifiable/Id,UnityEngine.GameObject>::get_Item(!0)
IL_0057: ldarg.0
IL_0058: ldfld LandPlot GardenCatcher::activator
IL_005d: callvirt UnityEngine.Transform UnityEngine.Component::get_transform()
IL_0062: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_position()
IL_0067: ldarg.0
IL_0068: ldfld LandPlot GardenCatcher::activator
IL_006d: callvirt UnityEngine.Transform UnityEngine.Component::get_transform()
IL_0072: callvirt UnityEngine.Quaternion UnityEngine.Transform::get_rotation()
IL_0077: call UnityEngine.Object UnityEngine.Object::Instantiate(UnityEngine.Object,UnityEngine.Vector3,UnityEngine.Quaternion)
IL_007c: isinst UnityEngine.GameObject
IL_0081: stloc.1
IL_0082: ldloc.1
IL_0083: callvirt !!0 UnityEngine.GameObject::AddComponent<DestroyAfterTime>()
IL_0088: pop
IL_0089: ldarg.0
IL_008a: ldfld LandPlot GardenCatcher::activator
IL_008f: ldloc.1
IL_0090: callvirt System.Void LandPlot::Attach(UnityEngine.GameObject)
IL_0095: ldarg.0
IL_0096: ldfld TutorialDirector GardenCatcher::tutDir
IL_009b: callvirt System.Void TutorialDirector::OnPlanted()
IL_00a0: ldarg.0
IL_00a1: ldfld UnityEngine.GameObject GardenCatcher::acceptFX
IL_00a6: ldnull
IL_00a7: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_00ac: brfalse IL_00d3
IL_00b1: ldarg.0
IL_00b2: ldfld UnityEngine.GameObject GardenCatcher::acceptFX
IL_00b7: ldarg.0
IL_00b8: call UnityEngine.Transform UnityEngine.Component::get_transform()
IL_00bd: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_position()
IL_00c2: ldarg.0
IL_00c3: call UnityEngine.Transform UnityEngine.Component::get_transform()
IL_00c8: callvirt UnityEngine.Quaternion UnityEngine.Transform::get_rotation()
IL_00cd: call UnityEngine.GameObject SRBehaviour::InstantiateDynamic(UnityEngine.GameObject,UnityEngine.Vector3,UnityEngine.Quaternion)
IL_00d2: pop
IL_00d3: ldarg.1
IL_00d4: callvirt UnityEngine.GameObject UnityEngine.Component::get_gameObject()
IL_00d9: call System.Void UnityEngine.Object::Destroy(UnityEngine.Object)
IL_00de: ret

*/
//  GardenCatcher.OnTriggerEnter
/*
IL_0000: ldarg.1
IL_0001: callvirt System.Boolean UnityEngine.Collider::get_isTrigger()
IL_0006: brfalse IL_000c
IL_000b: ret
IL_000c: ldarg.0
IL_000d: ldfld LandPlot GardenCatcher::activator
IL_0012: callvirt System.Boolean LandPlot::HasAttached()
IL_0017: brfalse IL_001d
IL_001c: ret
IL_001d: ldarg.1
IL_001e: callvirt !!0 UnityEngine.Component::GetComponent<Identifiable>()
IL_0023: stloc.0
IL_0024: ldloc.0
IL_0025: ldnull
IL_0026: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_002b: brfalse IL_00de
IL_0030: ldarg.0
IL_0031: ldfld System.Collections.Generic.Dictionary`2<Identifiable/Id,UnityEngine.GameObject> GardenCatcher::plantableDict
IL_0036: ldloc.0
IL_0037: ldfld Identifiable/Id Identifiable::id
IL_003c: callvirt System.Boolean System.Collections.Generic.Dictionary`2<Identifiable/Id,UnityEngine.GameObject>::ContainsKey(!0)
IL_0041: brfalse IL_00de
IL_0046: ldarg.0
IL_0047: ldfld System.Collections.Generic.Dictionary`2<Identifiable/Id,UnityEngine.GameObject> GardenCatcher::plantableDict
IL_004c: ldloc.0
IL_004d: ldfld Identifiable/Id Identifiable::id
IL_0052: callvirt !1 System.Collections.Generic.Dictionary`2<Identifiable/Id,UnityEngine.GameObject>::get_Item(!0)
IL_0057: ldarg.0
IL_0058: ldfld LandPlot GardenCatcher::activator
IL_005d: callvirt UnityEngine.Transform UnityEngine.Component::get_transform()
IL_0062: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_position()
IL_0067: ldarg.0
IL_0068: ldfld LandPlot GardenCatcher::activator
IL_006d: callvirt UnityEngine.Transform UnityEngine.Component::get_transform()
IL_0072: callvirt UnityEngine.Quaternion UnityEngine.Transform::get_rotation()
IL_0077: call UnityEngine.Object UnityEngine.Object::Instantiate(UnityEngine.Object,UnityEngine.Vector3,UnityEngine.Quaternion)
IL_007c: isinst UnityEngine.GameObject
IL_0081: stloc.1
IL_0082: ldloc.1
IL_0083: callvirt !!0 UnityEngine.GameObject::AddComponent<DestroyAfterTime>()
IL_0088: pop
IL_0089: ldarg.0
IL_008a: ldfld LandPlot GardenCatcher::activator
IL_008f: ldloc.1
IL_0090: callvirt System.Void LandPlot::Attach(UnityEngine.GameObject)
IL_0095: ldarg.0
IL_0096: ldfld TutorialDirector GardenCatcher::tutDir
IL_009b: callvirt System.Void TutorialDirector::OnPlanted()
IL_00a0: ldarg.0
IL_00a1: ldfld UnityEngine.GameObject GardenCatcher::acceptFX
IL_00a6: ldnull
IL_00a7: call System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)
IL_00ac: brfalse IL_00d3
IL_00b1: ldarg.0
IL_00b2: ldfld UnityEngine.GameObject GardenCatcher::acceptFX
IL_00b7: ldarg.0
IL_00b8: call UnityEngine.Transform UnityEngine.Component::get_transform()
IL_00bd: callvirt UnityEngine.Vector3 UnityEngine.Transform::get_position()
IL_00c2: ldarg.0
IL_00c3: call UnityEngine.Transform UnityEngine.Component::get_transform()
IL_00c8: callvirt UnityEngine.Quaternion UnityEngine.Transform::get_rotation()
IL_00cd: call UnityEngine.GameObject SRBehaviour::InstantiateDynamic(UnityEngine.GameObject,UnityEngine.Vector3,UnityEngine.Quaternion)
IL_00d2: pop
IL_00d3: ldarg.1
IL_00d4: callvirt UnityEngine.GameObject UnityEngine.Component::get_gameObject()
IL_00d9: call System.Void UnityEngine.Object::Destroy(UnityEngine.Object)
IL_00de: ret

*/
