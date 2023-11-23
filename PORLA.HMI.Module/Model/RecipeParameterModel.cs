using Google.Protobuf.WellKnownTypes;
using Prism.Mvvm;
using System.ComponentModel;
namespace PORLA.HMI.Module.Model
{
    public class RecipeParameterModel : BindableBase
    {
        private int _numberId = 0;
        public int NumberId
        {
            get { return _numberId; }
            set { SetProperty(ref _numberId, value); }
        }
        private string _recipeName = "";
        public string RecipeName
        {
            get { return _recipeName; }
            set { SetProperty(ref _recipeName, value); }
        }
        private string _xOriginalPosition = "";

        public string XOriginalPosition
        {
            get { return _xOriginalPosition; }
            set { SetProperty(ref _xOriginalPosition, value); }
        }

        private string _yOriginalPosition = "";
        public string YOriginalPosition
        {
            get { return _yOriginalPosition; }
            set { SetProperty(ref _yOriginalPosition, value); }
        }

        private string _dXPosition = "";
        public string DXPosition
        {
            get { return _dXPosition; }
            set { SetProperty(ref _dXPosition, value); }
        }

        private string _dYPosition = "";
        public string DYPosition
        {
            get { return _dYPosition; }
            set { SetProperty(ref _dYPosition, value); }
        }

        private string _rXPosition = "";
        public string RXPosition
        {
            get { return _rXPosition; }
            set { SetProperty(ref _rXPosition, value); }
        }

        private string _rYPosition = "";
        public string RYPosition
        {
            get { return _rYPosition; }
            set { SetProperty(ref _rYPosition, value); }
        }

        private string _speedAxisX = "";
        public string SpeedAxisX
        {
            get { return _speedAxisX; }
            set { SetProperty(ref _speedAxisX, value); }
        }

        private string _speedAxisY = "";
        public string SpeedAxisY
        {
            get { return _speedAxisY; }
            set { SetProperty(ref _speedAxisY, value); }
        }

        private string _sensorType = "";
        public string SensorType
        {
            get { return _sensorType; }
            set { SetProperty(ref _sensorType, value); }
        }

        private string _thicknessSelection = "";
        public string ThicknessSelection 
        {
            get { return _thicknessSelection; }
            set { SetProperty(ref _thicknessSelection, value); }
        }
        // DWD left: Detection Window Definition Left side
        private string _dWDLeft = "";
        public string DWDLeft
        {
            get { return _dWDLeft; }
            set { SetProperty(ref _dWDLeft, value); }
        }
        // DWD left: Detection Window Definition Right side
        private string _dWDRight = "";
        public string DWDRight
        {
            get { return _dWDRight; }
            set { SetProperty(ref _dWDRight, value); }
        }
        // QTH: Quality Threshold
        private string _qTH= "";
        public string QTH
        {
            get { return _qTH; }
            set { SetProperty(ref _qTH, value); }
        }

        #region Multipoint Properties
        private string _multiPointT1X;
        public string MultiPointT1X
        {
            get { return _multiPointT1X; }
            set { SetProperty(ref _multiPointT1X, value); }
        }
        private string _multiPointT1Y;
        public string MultiPointT1Y
        {
            get { return _multiPointT1Y; }
            set { SetProperty(ref _multiPointT1Y, value); }
        }
        private string _multiPointT2X;
        public string MultiPointT2X
        {
            get { return _multiPointT2X; }
            set { SetProperty(ref _multiPointT2X, value); }
        }
        private string _multiPointT2Y;
        public string MultiPointT2Y
        {
            get { return _multiPointT2Y; }
            set { SetProperty(ref _multiPointT2Y, value); }
        }
        private string _multiPointT3X;
        public string MultiPointT3X
        {
            get { return _multiPointT3X; }
            set { SetProperty(ref _multiPointT3X, value); }
        }
        private string _multiPointT3Y;
        public string MultiPointT3Y
        {
            get { return _multiPointT3Y; }
            set { SetProperty(ref _multiPointT3Y, value); }
        }
        private string _multiPointT4X;
        public string MultiPointT4X
        {
            get { return _multiPointT4X; }
            set { SetProperty(ref _multiPointT4X, value); }
        }
        private string _multiPointT4Y;
        public string MultiPointT4Y
        {
            get { return _multiPointT4Y; }
            set { SetProperty(ref _multiPointT4Y, value); }
        }
        private string _multiPointT5X;
        public string MultiPointT5X
        {
            get { return _multiPointT5X; }
            set { SetProperty(ref _multiPointT5X, value); }
        }
        private string _multiPointT5Y;
        public string MultiPointT5Y
        {
            get { return _multiPointT5Y; }
            set { SetProperty(ref _multiPointT5Y, value); }
        }
        private string _multiPointT6X;
        public string MultiPointT6X
        {
            get { return _multiPointT6X; }
            set { SetProperty(ref _multiPointT6X, value); }
        }
        private string _multiPointT6Y;
        public string MultiPointT6Y
        {
            get { return _multiPointT6Y; }
            set { SetProperty(ref _multiPointT6Y, value); }
        }
        private string _multiPointT7X;
        public string MultiPointT7X
        {
            get { return _multiPointT7X; }
            set { SetProperty(ref _multiPointT7X, value); }
        }
        private string _multiPointT7Y;
        public string MultiPointT7Y
        {
            get { return _multiPointT7Y; }
            set { SetProperty(ref _multiPointT7Y, value); }
        }
        private string _multiPointT8X;
        public string MultiPointT8X
        {
            get { return _multiPointT8X; }
            set { SetProperty(ref _multiPointT8X, value); }
        }
        private string _multiPointT8Y;
        public string MultiPointT8Y
        {
            get { return _multiPointT8Y; }
            set { SetProperty(ref _multiPointT8Y, value); }
        }
        private string _multiPointT9X;
        public string MultiPointT9X
        {
            get { return _multiPointT9X; }
            set { SetProperty(ref _multiPointT9X, value); }
        }
        private string _multiPointT9Y;
        public string MultiPointT9Y
        {
            get { return _multiPointT9Y; }
            set { SetProperty(ref _multiPointT9Y, value); }
        }
        private string _multiPointT10X;
        public string MultiPointT10X
        {
            get { return _multiPointT10X; }
            set { SetProperty(ref _multiPointT10X, value); }
        }
        private string _multiPointT10Y;
        public string MultiPointT10Y
        {
            get { return _multiPointT10Y; }
            set { SetProperty(ref _multiPointT10Y, value); }
        }
        private string _multiPointT11X;
        public string MultiPointT11X
        {
            get { return _multiPointT11X; }
            set { SetProperty(ref _multiPointT11X, value); }
        }
        private string _multiPointT11Y;
        public string MultiPointT11Y
        {
            get { return _multiPointT11Y; }
            set { SetProperty(ref _multiPointT11Y, value); }
        }
        private string _multiPointT12X;
        public string MultiPointT12X
        {
            get { return _multiPointT12X; }
            set { SetProperty(ref _multiPointT12X, value); }
        }
        private string _multiPointT12Y;
        public string MultiPointT12Y
        {
            get { return _multiPointT12Y; }
            set { SetProperty(ref _multiPointT12Y, value); }
        }
        private string _multiPointT13X;
        public string MultiPointT13X
        {
            get { return _multiPointT13X; }
            set { SetProperty(ref _multiPointT13X, value); }
        }
        private string _multiPointT13Y;
        public string MultiPointT13Y
        {
            get { return _multiPointT13Y; }
            set { SetProperty(ref _multiPointT13Y, value); }
        }
        private string _multiPointT14X;
        public string MultiPointT14X
        {
            get { return _multiPointT14X; }
            set { SetProperty(ref _multiPointT14X, value); }
        }
        private string _multiPointT14Y;
        public string MultiPointT14Y
        {
            get { return _multiPointT14Y; }
            set { SetProperty(ref _multiPointT14Y, value); }
        }
        private string _multiPointT15X;
        public string MultiPointT15X
        {
            get { return _multiPointT15X; }
            set { SetProperty(ref _multiPointT15X, value); }
        }
        private string _multiPointT15Y;
        public string MultiPointT15Y
        {
            get { return _multiPointT15Y; }
            set { SetProperty(ref _multiPointT15Y, value); }
        }
        private string _multiPointT16X;
        public string MultiPointT16X
        {
            get { return _multiPointT16X; }
            set { SetProperty(ref _multiPointT16X, value); }
        }
        private string _multiPointT16Y;
        public string MultiPointT16Y
        {
            get { return _multiPointT16Y; }
            set { SetProperty(ref _multiPointT16Y, value); }
        }
        #endregion

    }

}
