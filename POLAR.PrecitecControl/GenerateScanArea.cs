using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace POLAR.PrecitecControl
{
    public partial class FSSAreaScan : BindableBase
    {
        private void UpdateCoordinate()
        {
            /*
             * Certainly! The regular expression pattern x0=-\d+\.\d+,y0=-\d+\.\d+,x1=\d+\.\d+,y1=\d+\.\d+ can be broken down as follows:
            x0=-: This part matches the literal characters "x0=-". It's looking for the substring "x0=-" exactly as it is in the input string.
            \d+: This part matches one or more digits (0-9). \d is a shorthand for any digit, and + means one or more occurrences of it.
            \.: This matches a literal period (dot). Since a period is a special character in regex, 
            it needs to be escaped with a backslash to be treated as a literal period.
            y0=-: Similar to the first part, this matches the literal characters "y0=-".
            \d+\.\d+: Again, this matches one or more digits followed by a period and then one or more digits. 
            This is used to match decimal numbers like "-24.53" and "15.16".
            ,: This part matches a literal comma. It's used to separate the different values in the input string.
            x1=\d+\.\d+,y1=\d+\.\d+: Similar to the previous parts, this portion matches the values for "x1" and "y1" in the same format as "x0" and "y0".
            So, in summary, the entire regex pattern is looking for a specific pattern of values in the input string. 
            It expects the format "x0=-<number>,y0=-<number>,x1=<number>,y1=<number>". The numbers can be positive or negative decimal numbers, and they are separated by commas. 
            The regular expression ensures that it matches this specific format and captures the entire pattern for replacement.
             */
            string _scanProgram = @"
            init
            {
            	$SODX 65 66 67 68 69 256 82 83 264 272;
            	// 256 -  Thickness 1, 264 -  Thickness 2, 272 - Thickness 3, 83 Sample Counter, 82 Intensity, 65-69 - Encoder
            }

            fn main(scanFreq= 50000) //Frequency the mirror speed is based on, should for must purposes be the same as the SHZ setting of the CHRocodile
            {
            	setFrequency(50000)
            	exposure(50000, 50)	//50 KHz, LAI 50
            	dwd{				//DWD to exclude the peak around 0. Up to 16 blocks of DWD can be added
            		add(20,1500, 1)
            	}
            	rect(x0=-24.53,y0=-15.16,x1=25.47,y1=15.23,nCols=500,nRows=303,interp=0,waitAtBegin=15000,waitAtEnd=250,label=""AreaScan 1"")
            	//Scan Area is defined by the top left corner (x0, y0) and the bottom right corner of the coordinate system 
            }";

            double numx0 = 23.5;
            double numy0 = 21.5;
            double numx1 = 20.5;
            double numy1 = 26.5;

            string pattern = @"x0=-\d+\.\d+,y0=-\d+\.\d+,x1=\d+\.\d+,y1=\d+\.\d+";
            //string pattern1 = @"\$SODX\b\s+\d+(?:\s+\d+)*|\bx0=-?\d+\.\d+,y0=-?\d+\.\d+,x1=-?\d+\.\d+,y1=-?\d+\.\d+";

            string replacement = $"x0={numx0},y0={numy0},x1={numx1},y1={numy1}";

            string result = Regex.Replace(_scanProgram, pattern, replacement);

            //Console.WriteLine(result);
            //Console.ReadLine();
        }
    }
}
