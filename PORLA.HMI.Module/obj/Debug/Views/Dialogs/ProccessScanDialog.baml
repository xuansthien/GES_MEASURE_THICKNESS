   M S B A M L   `   `   `  ����K  GPORLA.HMI.Module, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullO,clr-namespace:PORLA.HMI.Module.Views.DialogsPORLA.HMI.Module.Views.Dialogs  5      7    1PORLA.HMI.Module.Views.Dialogs.ProccessScanDialog   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      >local,clr-namespace:PORLA.HMI.Module.Views.Dialogs     5      T PPrism.Wpf, Version=8.1.97.5141, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59$prismhttp://prismlibrary.com/  5      _ [MaterialDesignThemes.Wpf, Version=4.9.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5
      !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel$
  TrueX�5	      #  Prism.Services.Dialogs.Dialog   WindowStyle 5      �� 5   
    �� 
TargetType" �6   .�5      ��  �� Property �@�AllowsTransparency �� Value . 6B   �� 5       �@ WindowStartupLocation$ CenterScreen=�6C   �� 5       �@�
ResizeMode$ NoResize=�6+   �� 5       �@�ShowInTaskbar . 6.   �� 5       � ��$	 528��6&   64   �� 5       � ��$	 300��6'   65   �� 5       �@�WindowStyle$
 None=�6,   6;   �� 5       � ��
 ���� 6A   �� 5       � ��
 �   �6<   �� 5       �@�Topmost . 67   5      .��5      �� $��0��6   $��0q�5      
�������
��� � �.��5   
   � $��Stretch=�6   $��Stretch=�6+    � RowDefinitions 5      �� 5      $
��1.5* �6!   61   �� 5       $��* �6!   6.   �� 5!      $��* �6!   6.   �� 5"      $��* �6!   6.   �� 5#      $��* �6!   6.    � ColumnDefinitions 5%      �� 5&      $��* �6#   6/   �� 5'      $��* �6$   60   �� 5(      $��* �6$   60   �� 5)      $��* �6$   60   .+�5+      �� $��2q�6    �� CornerRadius$ 10��5,      $��4��5-      $��6��5.      ��5/      �� 50       �� EndPoint$ 0.5,1&�6*   	 �� 
StartPoint$	 0.5,0&�6;   .��51      � 
 � Color$
 	#FF5D5B5B��6'    � Offset$ 1Q�69   � 52      $
 	#FFF1EEEE��6'   $ 0Q�69   53      55      �� 5;      $��Processing...��6   $��35(�6-   $��1��6;   $��2��6K   $
��Wrap=�6_   $��Center=�5<      $��Center=�65    �� TextAlignment$ Center=�6P   $
��Bold"�6g   �� 5>      $��12(�60   $��3��6>   $
��Wrap=�6N   $��10q�6b   $��Right=�5?      $	��Top=�64   $ Center=�6L   $
��Bold"�6c   ��5>      �� *HMIver+5?   c   �� 5A      $��Center=�6   $��Center=�66    �� Orientation$ Vertical=�6Q   $��0��6h   $��4��6x   $��1��5B      $��3��6&   $��	10,0,10,0q�67   .+�5F      �� 
�������$��0 0 0 5q�6,   $��Center=�6=   ._�5G      � $��Center=�6    .+�5H      �� $��Center=�6$   $
 Left=�5I   $   $��13(�69   '  !MaterialDesignThemes.Wpf.PackIcon  5J         Kind$ 	Timelapse=�66   $��30��6G   $��30��6R   5K      �� 5L      $��80��6?   $��Center=�5M   $   $
 Left=�5N   $   $��30(�69   ��5L   $   �� *	CycleTime+5N   9   5O      5P      �� 5Q      $��Bottom=�6   
�������.@�5R      �� .��5S      � -   5        $��progressBar1��5S   >   $	��508��6&   $��20��62    � IsIndeterminate .  � Triggers 5T      :� 5U   "    :� RoutedEvent$ ProgressBar.Loaded��6/   .[�5V   &   �� .��5W   *   �� .��5X   .   W�  �� 
TargetName$ progressBar1��5Y   1    �� TargetProperty$ Value�5Z   1    g� RepeatBehavior$ Forever��5[   1   5\   +   5]   '   5^   #   5`      5a      5b      5c      �� 5d      $��5��6   $��0��5e      $��4��5f      $��Center=�5g      $��Center=�5h      $ 
Horizontal=�5i      .+�5j      �� $��0q�5k      $��Cancel��6$   $��Cancel��5m      ���	#FFD16137$��50��5n   0   $	��100��6<   y�5j      �� *VisibleButton+��5l      �� *CloseDialogCommand+5o      5p      5q      5r      5s      