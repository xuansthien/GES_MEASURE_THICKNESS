   M S B A M L   `   `   `  ���� 	  PORLA&     PORLA.HMI.Main.Views.Bottompanel   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     _ [MaterialDesignThemes.Wpf, Version=4.9.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  [ WPrism.DryIoc.Wpf, Version=8.1.97.5141, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59T PPrism.Wpf, Version=8.1.97.5141, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59&prismhttp://prismlibrary.com/   !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel$
  TrueX�   MaterialDesignBody#���   $��Regular"�$��13(�&   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=�   TextRenderingMode$
 Auto=����#EDEDED  MaterialDesignFont#���   � 	Resources   BlinkStoryboard �� TargetProperty	 �� To g� Duration g� AutoReverse g� RepeatBehavior��%v   &	       �� .���� $1 +(Button.Background).(SolidColorBrush.Color)�$	 RedX�$ 0:0:0.8D� . $ Forever��.��� 	 � ColumnDefinitions	 �� $��2* ��� $��1* �.+�� $��0��.+��� -   $��btnHome��$��HomePage��$��1q�$	��100��$	��100��$��Center=�
���iii����� *NavigateCommand+���� 
 �� 
TargetType"
 �� # MaterialDesignRaisedLightButton �� BasedOn# [ .���  �� Property � �� �� Value �#333333 �� Triggers ��  �� Binding �� *AppServiceInstance.pageLogin+ �� Value HomePage.a���  � �� �#0072C6���� $��55��$��90��$��Center=�$��Center=�.+��� -   $��txthome��$��13(�
�������$��Center=�$��Center=�$��0q����� "
 ��.���  � ��$
 Home�� ��  �� *AppServiceInstance.language+ 
Vietnamese.a���  � ��$ Trang Chủ��'  !MaterialDesignThemes.Wpf.PackIcon     Kind$
 Home=�$��Center=�$��Center=�
�������$��35��$��35���� -   $��	btnManual��$��ManualPageView��$��1q�$	��100��$	��100��$��Center=�
���iii����� *NavigateCommand+���� "
 ��# [ .���  � �� �#333333 ��  �� *AppServiceInstance.pageLogin+ ManualPageView.a���  � �� �#0072C6���� $��55��$��90��$��Center=�$��Center=�.+��� -   $��	txtManual��$��13(�
�������$��Center=�$��Center=�$��0q����� "
 ��.���  � ��$ Manual�� ��  �� *AppServiceInstance.language+ 
Vietnamese.a���  � ��$ 
Bằng Tay��  $ 
CursorHand=�$��Center=�$��Center=�
�������$��35��$��35���� -   $��
btnSetting��$��SettingPage��$��1q�$	��100��$	��100��$��Center=�
���iii����� *NavigateCommand+���� "
 ��# [ .���  � �� �#333333 ��  �� *AppServiceInstance.pageLogin+ SettingPage.a���  � �� �#0072C6���� $��55��$��90��$��Center=�$��Center=�.+��� -   $��txtSt��$��13(�
�������$��Center=�$��Center=�$��0q����� "
 ��.���  � ��$ Setting�� ��  �� *AppServiceInstance.language+ 
Vietnamese.a���  � ��$ Cài Đặt��  $ Tools=�$��Center=�$��Center=�
�������$��35��$��35���� -   $��btnStatistics��$��
TestResult��$��1q�$	��100��$	��100��$��Center=�
���iii����� *NavigateCommand+���� "
 ��# [ .���  � �� �#333333 ��  �� *AppServiceInstance.pageLogin+ 
TestResult.a���  � �� �#0072C6���� $��55��$��75��$��Center=�$��Center=�.+��� -   $��txtStat��$��13(�
�������$��Center=�$��Center=�$��0q����� "
 ��.���  � ��$ 	Statistic�� ��  �� *AppServiceInstance.language+ 
Vietnamese.a���  � ��$ Thống Kê��  $ ChartAreaspline=�$��Center=�$��Center=�
�������$��35��$��35���� $��MachineLogPage��$��1q�$	��100��$	��100��$��Center=�
���iii����� *NavigateCommand+���� "
 ��# [ .���  � �� �#333333 ��  �� *AppServiceInstance.pageLogin+ MachineLogPage.a���  � �� �#0072C6���� $��55��$��90��$��Center=�$��Center=�.+��� $	��Log��$��13(�
�������$��Center=�$��Center=�$��0q�  $ FileDocumentEditOutline=�$��Center=�$��Center=�
�������$��35��$��35��� $��1��$��Right=�.+��� -	   $��btnAlarm��$��	AlarmPage��$��1,1,2,0q�$	��100��$	��116��$��Center=�
���iii����� *NavigateCommand+���� "
 ��# [ .���  � �� �#333333 ��  �� *" AppServiceInstance.AlarmPresence+ True N� EnterActions �� .���� .���� $1 +(Button.Background).(SolidColorBrush.Color)�$	 RedX�$ 0:0:0.8D� . $ Forever����  �� *" AppServiceInstance.AlarmPresence+	 False.a���  � �� �#333333���� $��55��$��90��$��Center=�$��Center=�.+��� -
   $��txtalarm��$��13(�
�������$��Center=�$��Center=�$��0q����� "
 ��.���  � ��$ Alarm�� ��  �� *AppServiceInstance.language+ 
Vietnamese.a���  � ��$ Cảnh Báo��  $ AlarmMultiple=�$��Center=�$��Center=�
�������$��35��$��35��