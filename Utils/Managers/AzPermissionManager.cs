using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Karan.Churi;

namespace IdoMaster_GensouWorld.Utils
{
    public class AzPermissionManager : PermissionManager
    {
        private List<string> permission;

        public AzPermissionManager(List<string> permission)
        {
            this.permission = permission;
        }

        public override List<string> SetPermission()
        {
            if (permission == null || !permission.Any())
                return base.SetPermission();
            else
                return permission;
        }
    }
}