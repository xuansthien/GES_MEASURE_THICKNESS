   M S B A M L   `   `   `  ����K  GPORLA.HMI.Module, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullY1clr-namespace:PORLA.HMI.Module.Views.SettingPages#PORLA.HMI.Module.Views.SettingPages  5      I)clr-namespace:PORLA.HMI.Module.ViewModelsPORLA.HMI.Module.ViewModels  5      4    .PORLA.HMI.Module.Views.SettingPages.RecipePage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      _ [MaterialDesignThemes.Wpf, Version=4.9.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      ,winui using:Microsoft.UI.Xaml.Controls  5      =local1clr-namespace:PORLA.HMI.Module.Views.SettingPages  5      T PPrism.Wpf, Version=8.1.97.5141, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59$prismhttp://prismlibrary.com/  5	      2vm)clr-namespace:PORLA.HMI.Module.ViewModels  5      !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel$
  TrueX�5
         MaterialDesignBody#���   5      $��Regular"�5      $��13(�5      &   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=�5         TextRenderingMode$
 Auto=�5      ���#EDEDED  MaterialDesignFont#���  5      {�5      �� *$"IAppServiceSI.SettingModeActivated+.��5      �  � RowDefinitions 5   
   �� 5      $��1* �6   6*   �� 5      $��1* �6   6*   �� 5      $
��0.2* �6   6,   .+�5   
   �  � Orientation$ Vertical=�6   $��0��6+   .+�5      �� $��0,0,0,0q�6   
���iii�$��	3,0,0,3.5q�5       ._�5!      �� $
��Left=�6   $��Center=�5"      $
��Left=�5#      $	��200��5$      $
��Bold"�6(   
���iii�
�������$��14(�5'      $��  List Recipe��5(      5)      �� 5*      .+�5+      &   System.Windows.Controls.DataGrid     AutoGenerateColumns .     CanUserSortColumns .    SelectionMode$ Extended=�5-   5   $
��Auto=�5.         
IsReadOnly . ��5+      �� *
RecipeList+	 �� SelectedItem	 5,      �� *SelectedRecipe+
 �� Mode$
 TwoWay=�   Columns 50      0  *System.Windows.Controls.DataGridTextColumn  51      ,  &System.Windows.Controls.DataGridColumn   Header$	 No.��6-      Width5  /System.Windows.Controls.DataGridLengthConverter$
 0.5* 6W   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6:   �� *
NumberId+6W     52      $ Recipe Name��6-   $ * 6a    6B   �� *
RecipeName+6a     53      $ Sensor Type��6-   $ * 6a    6B   �� *
SensorType+6a   55      56      57      � 58   
   $ Vertical=�6   $��1��6+   .+�59      �� $��0,0,0,0q�6   
���iii�$��	3,0,0,3.5q�5;      ._�5<      �� $
��Left=�6   $��Center=�5=      $
��Left=�5>      $	��200��5?      $
��Bold"�6(   
���iii�
�������$��14(�5B      $��  Recipe Detail��5C      5D      �� 5E      .+�5F         .   . $ Single=�5G   Q   $
��Auto=�5H       . ��5F      �� *RecipeDetail+ 5J        5K      $ Recipe Name��6-   $ * 6a    6B   �� *
RecipeName+6a     5L      $ Sensor Type��6-   $ * 6a    6B   �� *
SensorType+6a     5M      $ Thickness Test��6-   $ * 6l    6E   �� *ThicknessSelection+6l     5N      $ 
X Original��6-   $ * 6g    6A   �� *XOriginalPosition+6g     5O      $ 
Y Original��6-   $ * 6g    6A   �� *YOriginalPosition+6g     5P      $ DX Distance��6-   $ * 6a    6B   �� *
DXPosition+6a     5Q      $ DY Distance��6-   $ * 6a    6B   �� *
DYPosition+6a     5R      $ RX Distance��6-   $ * 6a    6B   �� *
RXPosition+6a     5S      $ RY Distance��6-   $ * 6a    6B   �� *
RYPosition+6a     5T      $ VX Speed��6-   $ * 6^    6?   �� *
SpeedAxisX+6^     5U      $ VY Speed��6-   $ * 6^    6?   �� *
SpeedAxisY+6^   5W      5X      5Y      � 5Z   
   $��Bottom=�6   $��Right=�5[      $��2��60   $��10q�5\      .+�5]      �� .+�5^      ��   CustomButtonStyle#��[ 6>   ��6   �� *ShowPopupCommand+.��5_      ��  �� Orientation$ 
Horizontal=�6!   .+�5`      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ 	OpenInNew=�62   $��40��6C   $��50��6N   $��Center=�6Z   $��Center=�6u   �� 5a      $	��New��6$   $��Center=�6/   5b      5c      5d      �� 5e      .+�5f      �� #��[ 6F   ��6   �� *UpdateRecipeParameterCmd+.��5g      �� $ 
Horizontal=�6!   .+�5h        $ Update=�62   $��40��6@   $��50��6K   $��Center=�6W   $��Center=�6r   �� 5i      $
��Edit��6$   $��Center=�60   5j      5k      5l      �� 5m      .+�5n      �� #��[ 5o      ��5n      �� *DownloadRecipeCmd+.��5p      �� $ 
Horizontal=�6!   .+�5q        $ Download=�62   $��40��6B   $��40��6M   $��Center=�6Y   $��Center=�6t   �� 5r      $��Dowload��6$   $��Center=�63   5s      5t      5u      5�      5�      5�      