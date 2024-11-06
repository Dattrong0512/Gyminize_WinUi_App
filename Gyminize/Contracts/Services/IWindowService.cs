using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.VoiceCommands;

namespace Gyminize.Contracts.Services;

public interface IWindowService
{
    void SetIsResizable(bool isResizable);
    void SetIsMaximizable(bool isMaximizable);
    void SetWindowSize(int width, int height);
}
