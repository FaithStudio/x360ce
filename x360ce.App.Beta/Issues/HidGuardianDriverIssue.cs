﻿using JocysCom.ClassLibrary.Controls;
using JocysCom.ClassLibrary.Controls.IssuesControl;
using System.Linq;
using x360ce.Engine;

namespace x360ce.App.Issues
{
	class HidGuardianDriverIssue : IssueItem
	{
		public HidGuardianDriverIssue() : base()
		{
			Name = "HID Guardian Driver";
			FixName = "Install";
		}

		public override void CheckTask()
		{
			var haveVirtual = SettingsManager.UserGames.Items.Any(x => x.EmulationType == (int)EmulationType.Virtual && x.EnableMask > 0);
			var haveHidden = SettingsManager.UserDevices.Items.Any(x => x.IsHidden);
			// HID Guardian is required if virtual emulation is enabled and some devices must be hidden.
			var required = haveVirtual && haveHidden;
			if (!required)
			{
				SetSeverity(IssueSeverity.None);
				return;
			}
			var hid = DInput.VirtualDriverInstaller.GetHidGuardianDriverInfo();
			if (hid.DriverVersion == 0)
			{
				SetSeverity(IssueSeverity.Moderate, 0, "You need to install HID Guardian Driver to hide DirectInput controllers.");
				return;
			}
			SetSeverity(IssueSeverity.None);
		}
		public override void FixTask()
		{
			ControlsHelper.BeginInvoke(() =>
			{
				MainForm.Current.MainTabControl.SelectedTab = MainForm.Current.OptionsTabPage;
				MainForm.Current.OptionsPanel.MainTabControl.SelectedTab = MainForm.Current.OptionsPanel.HidGuardianTabPage;
			});
			//Program.RunElevated(AdminCommand.InstallHidGuardian);
			//ViGEm.HidGuardianHelper.InsertCurrentProcessToWhiteList();
		}

	}
}
