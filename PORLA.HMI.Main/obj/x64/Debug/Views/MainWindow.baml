   M S B A M L   `   `   `  ����	  PORLA%    PORLA.HMI.Main.Views.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   [ WPrism.DryIoc.Wpf, Version=8.1.97.5141, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59T PPrism.Wpf, Version=8.1.97.5141, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59&prismhttp://prismlibrary.com/   5   	   !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel$
  TrueX�5   	    � WindowStartupLocation$ CenterScreen=�5	   	    � WindowState$ 	Maximized=�5
   	    � WindowStyle$
 None=�5   	    � 
ResizeMode$ NoResize=�5   	    � Title$ PORLA��5   	   $
��1024��5   	   $
��1280��5   	   .��5      �  � RowDefinitions 5   
   �� 5      $
��1.3* �6   6,   �� 5      $	��12* �6   6+   �� 5      $
��0.9* �6   6,   .+�5   
   �� $��0��6   ���#414141$��0q�5      ._�5      �� $��0��6   !  Prism.Regions.RegionManager   
RegionName$ TopPanelRegion��5      5      �� 5   
   $��2��6   ���#414141$��0q�5      ._�5      �� $ BottomPanelRegion��6   5!      � 5"   
   $��1��6    � ColumnDefinitions 5#      �� 5$      $��11.5* �6#   63   �� 5%      $	��120 �6#   61    5'      �� 5(      $
��0.7* �6    60   �� 5)      $��* �6    6-   .+�5+      �� $��0��6   $��2��5,      
���iii�$��1q�5.      $��3,2,3,0q�5/      ���#D6D6D6._�51      �� $ ContentRegion��6!   53      �� 55      $��1��6   $��0��56      
���iii�$��1,1,1,0q�58      $��1,2,3,0q�59      ���#D6D6D6._�5;      �� $ OptionPanelRegion��6!   5=      �� 5?      $��1��6   $��1��5@      
���iii�$��1,0,1,1q�5B      $��1,0,3,0q�5C      ���#D6D6D6._�5F      �� $ RightPanelRegion��6!   5H      5J      5O      5P      