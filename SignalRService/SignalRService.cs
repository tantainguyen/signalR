﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;


namespace SignalRService
{
    partial class SignalRService : ServiceBase
    {
        public SignalRService()
        {
            InitializeComponent();
        }
        protected override void OnStart(string[] args)
        {            
           
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }
    }
}
