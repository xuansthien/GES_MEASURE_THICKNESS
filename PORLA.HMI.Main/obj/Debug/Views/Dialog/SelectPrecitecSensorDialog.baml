   M S B A M L   `   `   `  ����@  <PORLA, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:PORLA.HMI.Main.Views.DialogPORLA.HMI.Main.Views.Dialog  5      <    6PORLA.HMI.Main.Views.Dialog.SelectPrecitecSensorDialog   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ;local)clr-namespace:PORLA.HMI.Main.Views.Dialog     5      [ WPrism.DryIoc.Wpf, Version=8.1.97.5141, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59T PPrism.Wpf, Version=8.1.97.5141, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59&prismhttp://prismlibrary.com/   5      !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel$
  TrueX�5      #  Prism.Services.Dialogs.Dialog   WindowStyle 5	      �� 5
   
    �� 
TargetType" �6   .�5      ��  �� Property �@�AllowsTransparency �� Value . 6B   �� 5       �@ WindowStartupLocation$ CenterScreen=�6C   �� 5       �@�
ResizeMode$ 	CanResize=�6+   �� 5       �@�ShowInTaskbar . 6.   �� 5       � ��$	 300��6&   64   �� 5       � ��$	 280��6'   65   �� 5       �@�WindowStyle$
 None=�6,   6;   �� 5       � ��
 ���w�6D   �� 5       � ��
 �   �6<   �� 5       �@�Topmost . 67   5      .��5      �� $��1q�6   ._�5   
   �� .+�5      �� $��Center=�6   $��Center=�5      ��5      �� 5       �� EndPoint$ 0.5,1&�6*    �� 
StartPoint$ 0.5,0&�6;   .��5      �  � Color$ 	#FF1F5391��6'    � Offset$ 0Q�69   � 5      $ 	#FF1F5493��6'   $ 1Q�69   � 5      $ 	#FF6BA0E0��6'   $ 0.493Q�69   5       .��5"      �� $��14(�63   	 �� TextAlignment$	 Center=�6A   $
��Bold"�6X   ��6   �� *Title+6X   5#      �� 5$      $>��8 PLEASE SELECT PROPER SENSOR IS CONNECTED TO CONTROLER! ��6   $	 Center=�6X   $��10q�5%      $��20(�6$   $
��Bold"�62   $
��Wrap=�6D   $	��250��6X   � 5&      
 � Orientation$
 
Horizontal=�6   $��Center=�61   .+�5'      � $
 Vertical=�6   $��Center=�63   .+�5(      �� $
��0 10q�6   $��50��6*   $��30��65   $
��Bold"�6A   $��Italic$�6S   $	��FSS��5)      $��16(�6*   $��Center=�68   $��Center=�5*      � 5+      .+�5,      �� $��Center=�6#   $��Center=�6>    d� 	IsChecked 5-   #   �� *IsFss+ � LayoutTransform 5.      �� 5/   "    �� ScaleX$ 2Q�61    �� ScaleY$ 2Q�6<   51      52      53      � 54      $
 Vertical=�6   $��Center=�63   $��Center=�55      .+�56      �� $
��0 10q�6   $��50��6*   $��30��65   $
��Bold"�6A   $��Italic$�6S   $��1D��57      $��16(�6)   $��Center=�67   $��Center=�58      � 59      .+�5:      �� $��Center=�6#   $��Center=�6>    5;   #   �� *IsOneD+ 5<      �� 5=   "   $ 2Q�61   $ 2Q�6<   5?      5@      5A      � 5B      $
 Vertical=�6   $��Center=�63   $��Center=�5C      .+�5D      �� $
��0 10q�6   $��58��6*   $��30��65   $
��Bold"�6A   $��Italic$�6S   $��1D MP��5E      $��16(�6,   $
��Left=�6:   $��Center=�5F      � 5G      .+�5H      �� $��Center=�6#   $��Center=�6>    5I   #   �� *IsOneDMultiPoints+ 5J      �� 5K   "   $ 2Q�61   $ 2Q�6<   5M      5N      5O      5Q      �� 5R      ���#9FB6CD$��10 10 10 10q�6*   $��Ok��5S         CustomButtonStyle#��[  5U      ��5T      �� *CloseDialogCommand+5U      5V      5W      5Y      