using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatraxImaging.Models {
    public class ApplicationModel {
        public string AppId { get; }
        public string AppName { get; }
        public string AppNiceName { get; }

        public ApplicationModel(string appId, string appName, string appNiceName) {
            AppId = appId;
            AppName = appName;
            AppNiceName = appNiceName;
        }

    }
}
