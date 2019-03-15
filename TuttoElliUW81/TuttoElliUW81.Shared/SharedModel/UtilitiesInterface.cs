using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace TuttoElliUW81.SharedModel
{
    interface UtilitiesInterface
    {
        Boolean isOdd(int value);
        void enableAppBarButton(AppBarButton abb);
        void disableAppBarButton(AppBarButton abb);
        //void Splitter(string str, StackPanel sp);
    }
}
