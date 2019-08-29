using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Terminal.Gui;
using wttop.Widgets.Common;
using Mono.Terminal;
using wttop.Helpers;

namespace wttop.Widgets {
    
    public class CPUGraphs : Widget
    {    
        Label[] cpus;
        
        Bar2[] bars;
        
        ISystemInfo systemInfo;

        public CPUGraphs(string text, IServiceProvider serviceProvider) : base(text)
        {
            systemInfo = serviceProvider.GetService<ISystemInfo>();
            DrawWidget();
        }

        void DrawWidget()
        {
            var maxCpusCount = systemInfo.GetCPUsCount();
            
            cpus = new Label[maxCpusCount];
            bars = new Bar2[maxCpusCount];
            var offsetY = 1;
            for (var i = 0; i < maxCpusCount; i++)
            {    
                cpus[i] = new Label($"cpu-{i}: ") {
                        X = 1,
                        Y = offsetY
                    };

                bars[i] = 
                    new Bar2(Color.Green){
                        X = Pos.Right(cpus[i]),
                        Y = offsetY,
                        Width = Dim.Percent(75),
                        Height= Dim.Sized(1)
                    };

                offsetY += 2;
            }
        
            Add(cpus);
            Add(bars);
        }

        public override bool Update(MainLoop MainLoop)
        {
            var cpusUsage = systemInfo.GetCPUsUsage();
            for (var i = 0; i < bars.Length; i++)
            {
                bars[i].Update(MainLoop, cpusUsage.ElementAt(i).PercentageUsage);
            };
            return true;
        }
    }
}