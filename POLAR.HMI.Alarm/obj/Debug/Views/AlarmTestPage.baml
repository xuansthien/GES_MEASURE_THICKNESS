   M S B A M L   `   `   `  ����  POLAR.HMI.Alarm)    #POLAR.HMI.Alarm.Views.AlarmTestPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      T PPrism.Wpf, Version=8.1.97.5141, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59$prismhttp://prismlibrary.com/  5      !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel$
  TrueX�5      .��5      �  � RowDefinitions 5   
   �� 5      $
��0.8* �6   6,   �� 5	      $��7* �6   6*   �� 5
      $
��1.5* �6   6,   �� 5      $
��0.8* �6   6,   �� 5      $��7* �6   6*   �� 5      $
��1.7* �6   6,   .+�5   
   �� $��0��6   $��0,0,0,0q�5      
���iii�$��	3,0,0,3.5q�5      ._�5      �� $
��Left=�6   $��Center=�5      $
��Left=�5      $	��200��5      $
��Bold"�5      
���iii�
�������$��14(�5      $��  Alarm��5      5      �� 5   
   $��1��6   
���iii�$��2q�5      ._�5       &   System.Windows.Controls.DataGrid     AutoGenerateColumns .     CanUserSortColumns .     
IsReadOnly .    CanUserAddRows .     ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ * 5$      ���#EDEDED$��0q�5&      $��12(�5'      ��5(      �� *CurrentAlarmList+   Columns 5)      0  *System.Windows.Controls.DataGridTextColumn  5*      ,  &System.Windows.Controls.DataGridColumn   Header$ Alarm ID��6)   	   Width$
	 2.5* 5+   )   1  +System.Windows.Controls.DataGridBoundColumn
   Binding
 5,   )   �� *	AlarmId+ �� StringFormat$ 	AID{0:D3}��5.        5/      $ 
Alarm Time��6)   $	 3* 50   )   
 51   )   �� *AlarmCreateTime+$ {0:dd/MM/yyyy HH:mm:ss}��   ElementStyle 52      �� 53       �� 
TargetType" ��6$   .�54   "   ��  �� Property � �� �� Value$ Center=�55   )   56      58        59      $ Category��6)   $	 3* 5:   )   
 5;   )   �� *AlarmCategory+ 5<      �� 5=      " ��6$   .�5>   "   ��  � ��$ Center=�5?   )   5@      5B        5C      $ Source��6)   $	 3* 5D   )   
 5E   )   �� *SourceModule+ 5F      �� 5G      " ��6$   .�5H   "   ��  � ��$ Center=�5I   )   5J      5L        5M      $ Description��6)   $	 5* 5N   )   
 5O   )   �� *Description+ 5P      �� 5Q      " ��6$   .�5R   "   ��  � ��$ Center=�5S   )   5T      5V        5W      $ Instruction��6)   $	 5* 5X   )   
 5Y   )   �� *Instruction+ 5Z      �� 5[      " ��6$   .�5\   "   ��  � ��$ Center=�5]   )   5^      5`       � 	Resources 5b      =  7System.Windows.Controls.Primitives.DataGridColumnHeader��%Z   '        �� 5c      "  6   .�5d      ��  � ��$ Center=�5e   !   5f      5g      5h      5i      5j      5l      