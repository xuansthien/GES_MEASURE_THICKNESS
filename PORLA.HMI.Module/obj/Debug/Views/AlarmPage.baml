   M S B A M L   `   `   `  ����K  GPORLA.HMI.Module, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:PORLA.HMI.Module.ViewModelsPORLA.HMI.Module.ViewModels  5   5   &     PORLA.HMI.Module.Views.AlarmPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      _ [MaterialDesignThemes.Wpf, Version=4.9.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      Eintr:http://schemas.microsoft.com/expression/2010/interactivity  5      T PPrism.Wpf, Version=8.1.97.5141, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59$prismhttp://prismlibrary.com/  5      :
viewmodels)clr-namespace:PORLA.HMI.Module.ViewModels  65   !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel$
  TrueX�5	         MaterialDesignBody#���   5
      $��Regular"�5      $��13(�5      &   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=�5         TextRenderingMode$
 Auto=�5      ���#EDEDED  MaterialDesignFont#���  5       � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $e _pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Light.xaml?�6%   �� 5      $h bpack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Defaults.xaml?�6%   �� 5      $z tpack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.DeepPurple.xaml?�6%   �� 5      $s mpack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor.Lime.xaml?�6%   �� 5      $ /Resource/styleGui.xaml?�6%   %    5      .��5      �  � RowDefinitions 5   
   �� 5       $
��0.8* �6   6,   �� 5!      $��7* �6   6*   �� 5"      $
��1.5* �6   6,   �� 5#      $
��0.8* �6   6,   �� 5$      $��7* �6   6*   �� 5%      $
��1.7* �6   6,   .+�5'   
   �� $��0��6   $��0,0,0,0q�5(      
���iii�$��	3,0,0,3.5q�5*      ._�5+      �� $
��Left=�6   $��Center=�5,      $
��Left=�5-      $	��200��5.      $
��Bold"�5/      
���iii�
�������$��14(�52      $��  Alarm��53      54      �� 55   
   $��1��6   
���iii�$��2q�57      ._�58      &   System.Windows.Controls.DataGrid     AutoGenerateColumns .     CanUserSortColumns .  	   
IsReadOnly	 . 
   CanUserAddRows
 .     ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ * 5<      ���#EDEDED$��0q�5>      $��12(�5?      ��5@      �� *CurrentAlarmList+ �� SelectedItem 5A      �� *SelectedAlarmItems+   Columns 5B      0  *System.Windows.Controls.DataGridTextColumn  5C      ,  &System.Windows.Controls.DataGridColumn   Header$ Alarm ID��6)      Width$
 2.5* 5D   )   1  +System.Windows.Controls.DataGridBoundColumn   Binding 5E   )   �� *	AlarmId+ �� StringFormat$ 	AID{0:D3}��5G        5H      $ 
Alarm Time��6)   $ 3* 5I   )    5J   )   �� *AlarmCreateTime+$ {0:dd/MM/yyyy HH:mm:ss}��   ElementStyle 5K      �� 5L       �� 
TargetType" ��6$   .�5M   "   ��  �� Property � �� �� Value$ Center=�5N   )   5O      5Q        5R      $ Category��6)   $ 3* 5S   )    5T   )   �� *AlarmCategory+ 5U      �� 5V      " ��6$   .�5W   "   ��  � ��$ Center=�5X   )   5Y      5[        5\      $ Source��6)   $ 3* 5]   )    5^   )   �� *SourceModule+ 5_      �� 5`      " ��6$   .�5a   "   ��  � ��$ Center=�5b   )   5c      5e        5f      $ Description��6)   $ 5* 5g   )    5h   )   �� *Description+ 5i      �� 5j      " ��6$   .�5k   "   ��  � ��$ Center=�5l   )   5m      5o        5p      $ Instruction��6)   $ 5* 5q   )    5r   )   �� *Instruction+ 5s      �� 5t      " ��6$   .�5u   "   ��  � ��$ Center=�5v   )   5w      5y       5{      =  7System.Windows.Controls.Primitives.DataGridColumnHeader��%Z   '        �� 5|      "  6   .�5}      ��  � ��$ Center=�5~   !   5      5�      5�      5�      � 5�   
   $��2��6   .+�5�      � .+�5�      �� -   5        $��btnCl��5�      ���#9FB6CD$��13(�5�      $��3q�5�      $��45��5�      $	��120��5�      
������ $��Reset Alarm��5�      ��5�      �� *ResetAlarmCmd+5�      �� 5�      ���#9FB6CD$��3q�5�      $��13(�5�      $��45��5�      $	��120��5�      
������ $��	Reset All��5�      ��5�      �� *ResetAllAlarmCmd+5�      5�      5�      �� 5�   
   $��3��6   $��0,0,0,0q�5�      
���iii�$��	3,0,0,3.5q�5�      ._�5�      �� $
��Left=�6   $��Center=�5�      $
��Left=�5�      $	��200��5�      $
��Bold"�5�      
���iii�
�������$��14(�5�      $��   History Alarm��5�      5�      �� 5�   
   $��4��6   
���iii�$��2q�5�      ._�5�        -   5        $��lstBarcode1��5�       .   .  	 . 
 .  $ * 5�      $�� 0q�5�      ���#EDEDED$��13(�5�      ��5�      �� *HistoryAlarmList+ 5�        5�      $ Alarm ID��6)   $ 3* 5�   )    5�   )   �� *	AlarmId+ 5�      �� 5�      " ��6$   .�5�   "   ��  � ��$ Center=�5�   )   5�      5�        5�      $ 
Alarm Time��6)   $ 3* 5�   )    5�   )   �� *AlarmCreateTime+ 5�      �� 5�      " ��6$   .�5�   "   ��  � ��$ Center=�5�   )   5�      5�        5�      $ Alarm Cleared Time��6)   $ 3* 5�   )    5�   )   �� *AlarmClearTime+ 5�      �� 5�      " ��6$   .�5�   "   ��  � ��$ Center=�5�   )   5�      5�        5�      $ Category��6)   $ 3* 5�   )    5�   )   �� *AlarmCategory+ 5�      �� 5�      " ��6$   .�5�   "   ��  � ��$ Center=�5�   )   5�      5�        5�      $ Source��6)   $ 3* 5�   )    5�   )   �� *SourceModule+ 5�      �� 5�      " ��6$   .�5�   "   ��  � ��$ Center=�5�   )   5�      5�        5�      $ Description��6)   $ 5* 5�   )    5�   )   �� *Description+ 5�      �� 5�      " ��6$   .�5�   "   ��  � ��$ Center=�5�   )   5�      5       5     ��%Z   '        �� 5     "  6   .�5     ��  � ��$ Center=�5  !   5     5     5     5	     5Z     5[     