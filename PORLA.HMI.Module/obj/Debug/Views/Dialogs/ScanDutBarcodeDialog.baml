   M S B A M L   `   `   `  ����K  GPORLA.HMI.Module, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullO,clr-namespace:PORLA.HMI.Module.Views.DialogsPORLA.HMI.Module.Views.Dialogs  5      9    3PORLA.HMI.Module.Views.Dialogs.ScanDutBarcodeDialog   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      >local,clr-namespace:PORLA.HMI.Module.Views.Dialogs     5      T PPrism.Wpf, Version=8.1.97.5141, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59$prismhttp://prismlibrary.com/  5      !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel$
  TrueX�5	      #  Prism.Services.Dialogs.Dialog   WindowStyle 5
      �� 5   
    �� 
TargetType" �6   .�5      ��  �� Property �@�AllowsTransparency �� Value . 6B   �� 5       �@ WindowStartupLocation$ CenterScreen=�6C   �� 5       �@�
ResizeMode$ 	CanResize=�6+   �� 5       �@�ShowInTaskbar . 6.   �� 5       � ��$	 300��6&   64   �� 5       � ��$	 200��6'   65   �� 5       �@�WindowStyle$
 None=�6,   6;   �� 5       � ��
 ���w�6D   �� 5       � ��
 �   �6<   �� 5       �@�Topmost . 67   5      .��5      � .+�5   
   �� .+�5      �� $��Center=�6   $��Center=�5      ��5      �� 5       �� EndPoint$ 0.5,1&�6*    �� 
StartPoint$ 0.5,0&�6;   .��5      �  � Color$ 	#FF1F5391��6'    � Offset$ 0Q�69   � 5      $ 	#FF1F5493��6'   $ 1Q�69   � 5       $ 	#FF6BA0E0��6'   $ 0.493Q�69   5!      .��5#      �� $��14(�63   	 �� TextAlignment$	 Center=�6A   $
��Bold"�6X   ��6   �� *Title+6X   5$      �� 5%      $�� Scan Barcode ��6   $��10q�6.   $��20(�6:   $
��Bold"�6H   �� 5&      $��Stretch=�5'      $��10q�64   $��20(�6@   $
��Bold"�6N   
 �� TextAlignment$
 Center=�5(      ��5&      �� *AppService.DutBarcode+ �� Mode$ TwoWay=�5(      �� 5)       �� Orientation$ 
Horizontal=�6   $��Center=�62   $��Center=�5*      .+�5+      �� ���#9FB6CD$��10q�6.   $��Ok��5,      $��12(�5-      $��35��6'   $��90��63   $��ok��5/         CustomButtonStyle#��[  50      ��5.      �� *CloseDialogCommand+50      �� 51      ���#9FB6CD$��10q�6.   $��12(�52      $��35��6'   $��90��63   $
��Exit��53      $��Cancel��54      #��[  55      ��53   (   �� *CloseDialogCommand+55      57      58      59      5:      