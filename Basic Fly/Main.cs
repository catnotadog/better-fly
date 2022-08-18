using System.Collections;
using MelonLoader;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC;

namespace Fly
{
    // Token: 0x02000002 RID: 2
    internal class Main : MelonMod
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public override void OnApplicationStart()
		{
			MelonCoroutines.Start(waitforui());
			
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020C8 File Offset: 0x000002C8
		private Transform camera()
		{
			return VRCPlayer.field_Internal_Static_VRCPlayer_0.transform;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000205F File Offset: 0x0000025F
		private IEnumerator waitforui()
		{
			MelonLogger.Msg("Waiting For Ui");
			while (GameObject.Find("UserInterface") == null)
			{
				yield return null;
			}
			while (GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)") == null)
			{
				yield return null;
			}
			MelonLogger.Msg("Ui loaded");
			var toinst = GameObject.Find("/UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/Wing_Right/Container/InnerContainer/WingMenu/ScrollRect/Viewport/VerticalLayoutGroup/Button_Emotes");
			var inst = GameObject.Instantiate(toinst, toinst.parent).gameObject;
			inst.name = "Button fly";
			var txt = inst.transform.Find("Container/Text_QM_H3").GetComponent<TextMeshProUGUI>();
			txt.richText = true;
			txt.text = "<color=#000080ff>Fly</color>";
			UnityEngine.Object.DestroyImmediate(inst.transform.Find("Container/Icon").gameObject);
			Button btn = inst.GetComponent<Button>();
			btn.onClick.RemoveAllListeners();
			btn.onClick.AddListener(delegate()
			{
				flytoggle = !flytoggle;
				if (!flytoggle)
				{
					txt.text = "<color=#000080ff>Fly</color>";
				}
				else
				{
					txt.text = "<color=#ff0000ff>Fly</color>";
				}
                
			});
			yield break;
		}


	// Token: 0x06000004 RID: 4 RVA: 0x000020E4 File Offset: 0x000002E4
	public override void OnUpdate()
        {
			if (flytoggle)
            {
				var bullshit2 = new CharacterController();
				bullshit2.velocity.Set(0, 0, 0);
			}
			if (!flytoggle)
			{
				if (loaded)
				{
					Physics.gravity = _originalGravity;
				}
			}
			else if (flytoggle && !(Physics.gravity == Vector3.zero))
			{
				_originalGravity = Physics.gravity;
				Physics.gravity = Vector3.zero;
				return;
			}
			if (flytoggle && !(Player.prop_Player_0 == null))
			{
				float num = Input.GetKey(KeyCode.LeftShift) ? (Time.deltaTime * 15f) : (Time.deltaTime * 10f);
				if (Player.prop_Player_0.field_Private_VRCPlayerApi_0.IsUserInVR())
				{
					if (Input.GetAxis("Oculus_CrossPlatform_SecondaryThumbstickVertical") < -0.5f)
					{
						Player.prop_Player_0.transform.position -= camera().up * num;
					}
					if (Input.GetAxis("Oculus_CrossPlatform_SecondaryThumbstickVertical") > 0.5f)
					{
						Player.prop_Player_0.transform.position += camera().up * num;
					}
					if (Input.GetAxis("Vertical") != 0f)
					{
						Player.prop_Player_0.transform.position += camera().forward * (num * Input.GetAxis("Vertical"));
					}
					if (Input.GetAxis("Horizontal") != 0f)
					{
						Player.prop_Player_0.transform.position += camera().transform.right * (num * Input.GetAxis("Horizontal"));
						return;
					}
				}
			}
			
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000207E File Offset: 0x0000027E
		public override void OnSceneWasInitialized(int buildIndex, string sceneName)
		{
			if (buildIndex == -1)
			{
				this._originalGravity = Physics.gravity;

				this.loaded = true;
			}
		}

		// Token: 0x04000001 RID: 1
		private bool flytoggle = false;

		// Token: 0x04000002 RID: 2


		// Token: 0x04000003 RID: 3
		private Vector3 _originalGravity;

		// Token: 0x04000004 RID: 4


		// Token: 0x04000005 RID: 5
		private bool loaded;
	}
}
