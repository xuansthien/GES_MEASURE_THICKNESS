   M S B A M L   `   `   `  ����K  GPORLA.HMI.Module, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullY1clr-namespace:PORLA.HMI.Module.Views.SettingPages#PORLA.HMI.Module.Views.SettingPages  5      4    .PORLA.HMI.Module.Views.SettingPages.RecipePage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      _ [MaterialDesignThemes.Wpf, Version=4.9.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      ,winui using:Microsoft.UI.Xaml.Controls  5      =local1clr-namespace:PORLA.HMI.Module.Views.SettingPages  5      T PPrism.Wpf, Version=8.1.97.5141, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59$prismhttp://prismlibrary.com/  5	      !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel$
  TrueX�5
         MaterialDesignBody#���   5      $��Regular"�5      $��13(�5      &   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=�5         TextRenderingMode$
 Auto=�5      ���#EDEDED  MaterialDesignFont#���  5      {�5      �� *$"IAppServiceSI.SettingModeActivated+.��5      �  � RowDefinitions 5   
   �� 5      $��1* �6   6*   �� 5      $��1* �6   6*   �� 5      $
��0.2* �6   6,   .+�5   
   �  � Orientation$ Vertical=�6   $��0��6+   .+�5)      �� $��Stretch=�6   .+�5*      &   System.Windows.Controls.DataGrid     AutoGenerateColumns .  ��. $��Visible=�5-      $��Visible=�5.         
IsReadOnly . ��5*      �� *
RecipeList+ �� SelectedItem 5,      �� *SelectedRecipe+   Columns 50      0  *System.Windows.Controls.DataGridTextColumn  51      ,  &System.Windows.Controls.DataGridColumn	   Header$		 No.��6-   
   Width5  /System.Windows.Controls.DataGridLengthConverter$
 * 6W   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6:   �� *
NumberId+6W     52      $	 Recipe Name��6-   $
 * 6a    6B   �� *
RecipeName+6a     53      $	 Sensor Type��6-   $
 * 6a    6B   �� *
SensorType+6a   55      56      57      � 58   
   $ Vertical=�6   $��1��6+   .+�59      �� $��0,0,0,0q�6   
���iii�$��	3,0,0,3.5q�5;      ._�5<      �� $
��Left=�6   $��Center=�5=      $
��Left=�5>      $	��200��5?      $
��Bold"�6(   
���iii�
�������$��14(�5A   4   $��  Recipe Detail��5B      5C      �� 5D      .+�5E         .     CanUserSortColumns .    SelectionMode$ Single=�5F   Q   $
��Auto=�5G       . ��5E      �� *RecipeDetail+y�6@   �� *IsOneD+ 5I        5J      $	 Recipe Name��6-   $
 * 6a    6B   �� *
RecipeName+6a     5K      $	 Sensor Type��6-   $
 * 6a    6B   �� *
SensorType+6a     5M      $	 X Center��6-   $
 * 6e    6?   �� *XOriginalPosition+6e     5N      $	 Y Center��6-   $
 * 6e    6?   �� *YOriginalPosition+6e     5O      $	 
Width Area��6-   $
 * 6`    6A   �� *
DXPosition+6`     5P      $	 Height Area��6-   $
 * 6a    6B   �� *
DYPosition+6a     5Q      $	 RX Distance��6-   $
 * 6a    6B   �� *
RXPosition+6a     5R      $	 RY Distance��6-   $
 * 6a    6B   �� *
RYPosition+6a     5S      $	 VX Speed��6-   $
 * 6^    6?   �� *
SpeedAxisX+6^     5T      $	 VY Speed��6-   $
 * 6^    6?   �� *
SpeedAxisY+6^   5V        5W       .   . $ Single=�5X   Q   $
��Auto=�5Y       . ��5W      �� *RecipeDetail+y�6@   �� *IsFSS+ 5[        5\      $	 Recipe Name��6-   $
 * 6a    6B   �� *
RecipeName+6a     5]      $	 Sensor Type��6-   $
 * 6a    6B   �� *
SensorType+6a     5_      $	 X Center��6-   $
 * 6e    6?   �� *XOriginalPosition+6e     5`      $	 Y Center��6-   $
 * 6e    6?   �� *YOriginalPosition+6e     5a      $	 
Width Area��6-   $
 * 6`    6A   �� *
DXPosition+6`     5b      $	 Height Area��6-   $
 * 6a    6B   �� *
DYPosition+6a     5c      $	 No. Of Points��6-   $
 * 6c    6D   �� *
RXPosition+6c     5d      $	 No. Of Lines��6-   $
 * 6b    6C   �� *
RYPosition+6b     5e      $	 VX Speed��6-   $
 * 6^    6?   �� *
SpeedAxisX+6^     5f      $	 VY Speed��6-   $
 * 6^    6?   �� *
SpeedAxisY+6^   5h        5i       .   . $ Single=�5k      $
��Auto=�5l       . ��5i      �� *RecipeDetail+ 5n        5o      $	 DWD Left Side��6-   $
 * 6`    6D   �� *	DWDLeft+6`     5p      $	 DWD Right Side��6-   $
 * 6b    6E   �� *
DWDRight+6b     5q      $	 Quality Threshold��6-   $
 * 6`    6H   �� *QTH+6`   5s        5t       .   . $ Single=�5v      $
��Auto=�5w       . y�5t      �� *
IsOneDMP+��6;   �� *RecipeDetail+ 5y        5z      $
	 T1_X��6-   $
 * 6]    6;   �� *MultiPointT1X+6]     5{      $
	 T1_Y��6-   $
 * 6]    6;   �� *MultiPointT1Y+6]     5|      $
	 T2_X��6-   $
 * 6]    6;   �� *MultiPointT2X+6]     5}      $
	 T2_Y��6-   $
 * 6]    6;   �� *MultiPointT2Y+6]     5~      $
	 T3_X��6-   $
 * 6]    6;   �� *MultiPointT3X+6]     5      $
	 T3_Y��6-   $
 * 6]    6;   �� *MultiPointT3Y+6]     5�      $
	 T4_X��6-   $
 * 6]    6;   �� *MultiPointT4X+6]     5�      $
	 T4_Y��6-   $
 * 6]    6;   �� *MultiPointT4Y+6]     5�      $
	 T5_X��6-   $
 * 6]    6;   �� *MultiPointT5X+6]     5�      $
	 T5_Y��6-   $
 * 6]    6;   �� *MultiPointT5Y+6]     5�      $
	 T6_X��6-   $
 * 6]    6;   �� *MultiPointT6X+6]     5�      $
	 T6_Y��6-   $
 * 6]    6;   �� *MultiPointT6Y+6]     5�      $
	 T7_X��6-   $
 * 6]    6;   �� *MultiPointT7X+6]     5�      $
	 T7_Y��6-   $
 * 6]    6;   �� *MultiPointT7Y+6]     5�      $
	 T8_X��6-   $
 * 6]    6;   �� *MultiPointT8X+6]     5�      $
	 T8_Y��6-   $
 * 6]    6;   �� *MultiPointT8Y+6]   5�        5�       .   . $ Single=�5�      $
��Auto=�5�       . y�5�      �� *
IsOneDMP+��6;   �� *RecipeDetail+ 5�        5�      $
	 T9_X��6-   $
 * 6]    6;   �� *MultiPointT9X+6]     5�      $
	 T9_Y��6-   $
 * 6]    6;   �� *MultiPointT9Y+6]     5�      $	 T10_X��6-   $
 * 6_    6<   �� *MultiPointT10X+6_     5�      $	 T10_Y��6-   $
 * 6_    6<   �� *MultiPointT10Y+6_     5�      $	 T11_X��6-   $
 * 6_    6<   �� *MultiPointT11X+6_     5�      $	 T11_Y��6-   $
 * 6_    6<   �� *MultiPointT11Y+6_     5�      $	 T12_X��6-   $
 * 6_    6<   �� *MultiPointT12X+6_     5�      $	 T12_Y��6-   $
 * 6_    6<   �� *MultiPointT12Y+6_     5�      $	 T13_X��6-   $
 * 6_    6<   �� *MultiPointT13X+6_     5�      $	 T13_Y��6-   $
 * 6_    6<   �� *MultiPointT13Y+6_     5�      $	 T14_X��6-   $
 * 6_    6<   �� *MultiPointT14X+6_     5�      $	 T14_Y��6-   $
 * 6_    6<   �� *MultiPointT14Y+6_     5�      $	 T15_X��6-   $
 * 6_    6<   �� *MultiPointT15X+6_     5�      $	 T15_Y��6-   $
 * 6_    6<   �� *MultiPointT15Y+6_     5�      $	 T16_X��6-   $
 * 6_    6<   �� *MultiPointT16X+6_     5�      $	 T16_Y��6-   $
 * 6_    6<   �� *MultiPointT16Y+6_   5�      5�      5�      � 5�   
   $��Bottom=�6   $��Right=�5�      $��2��60   $��10q�5�      .+�5�      �� .+�5�      ��   CustomButtonStyle#��[ 6>   ��6   �� *ShowPopupCommand+.��5�      ��  �� Orientation$ 
Horizontal=�6!   .+�5�      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ 	OpenInNew=�62   $��40��6C   $��50��6N   $��Center=�6Z   $��Center=�6u   �� 5�      $	��New��6$   $��Center=�6/   5�      5�      5�      �� 5�      .+�5�      �� #��[ 6F   ��6   �� *UpdateRecipeParameterCmd+.��5�      �� $ 
Horizontal=�6!   .+�5�        $
 Edit=�62   $��40��6>   $��50��6I   $��Center=�6U   $��Center=�6p   �� 5�      $
��Edit��6$   $��Center=�60   5�      5�      5�      �� 5�      .+�5�      �� #��[ 5�      ��5�      �� *DownloadRecipeCmd+.��5�      �� $ 
Horizontal=�6!   .+�5�        $ Download=�62   $��40��6B   $��40��6M   $��Center=�6Y   $��Center=�6t   �� 5�      $��Dowload��6$   $��Center=�63   5�      5�      5�      �� 5�      .+�5�      �� #��[ 5�      ��5�      �� *DeleteRecipeParameterCmd+.��5�      �� $ 
Horizontal=�6!   .+�5�        $ Delete=�62   $��40��6@   $��40��6K   $��Center=�6W   $��Center=�6r   �� 5�      $��Delete��6$   $��Center=�62   5�      5�      5�      5�      5�      5�      