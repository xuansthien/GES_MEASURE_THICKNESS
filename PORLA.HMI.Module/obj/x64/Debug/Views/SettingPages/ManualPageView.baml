   M S B A M L   `   `   `  ����K  GPORLA.HMI.Module, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullY1clr-namespace:PORLA.HMI.Module.Views.SettingPages#PORLA.HMI.Module.Views.SettingPages  5      8    2PORLA.HMI.Module.Views.SettingPages.ManualPageView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Clocal1clr-namespace:PORLA.HMI.Module.Views.SettingPages     5      _ [MaterialDesignThemes.Wpf, Version=4.9.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      T PPrism.Wpf, Version=8.1.97.5141, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59$prismhttp://prismlibrary.com/  5
      _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5      ���#EDEDED!  Prism.Mvvm.ViewModelLocator    AutoWireViewModel$
  TrueX�5      .��5      �  � RowDefinitions 5   
   �� 5      $��0* �6   6*   �� 5      $��7* �6   6*   .+�5   
   � $��1��6   $��0,10,0,0q�5       � ColumnDefinitions 5      �� 5      $��1* �6#   60   �� 5      $��2* �6#   60    5      �� 5      $
��auto �6    60   �� 5      $��* �6    6-   �� 5      $
��auto �6    60   �� 5      $
��1.3* �6    60   .+�5      �� $��1��6   $��7,0,7,5q�5       
���iii�$��0,0,0,1q�5"      ._�5#      �� $
��Left=�6   $��Center=�5$      $
��Left=�5%      $	��200��5&      
������ 
���   �$��17(�5)      $��Normal"�5*      $�� Input��5+      5,      �� 5-      $��2��6   $��7,0,7,5q�5.      
���iii�$��0,0,0,1q�50      ._�51      �� $
��Left=�6   $��Center=�52      $
��Left=�53      $	��200��54      
������ 
���   �$��17(�57      $��Normal"�58      $��
Motion IAI��59      5:      � 5;      $��1��6   $��0��6     5<      �� 5=      $��1* �6'   �� 5>      $��1* �6'   .+�5@      �� $��0��6    �� Orientation$ Vertical=�6-   $��10,5,0,0q�6D   .+�5B      �  � Orientation$ 
Horizontal=�6    $��Center=�5C       .+�5D      �� $��Right=�6"   $��0,0,7,0q�5E   "   $��30��5F   "   4  .System.Windows.Automation.AutomationProperties   AutomationId$ 	LampStart��5G   "   ��5H      �� 5I   "    �� 
TargetType" ��6(    	  Lampt �� BasedOn# [  5J   (   .�5K   &   ��  �� Property �@�Tag	 �� Value$	 False��6<   
 �� Triggers
 5L   &   �� 5M   *    �� Binding 66   �� *$"AppService.DIOModule1.IOMapping[4]+ �� Value True5N   6   .a�5O   .   ��  �@�Tag$
	 True��6D   5P   +   5R   #   5T      �� 5U      $��Start��6$   $��Center=�5V   $   $��13(�5W   $   5X      � 5Y      $ 
Horizontal=�6    $	��0,5q�5Z       $��Center=�5[       .+�5\      �� $��Right=�6"   $��0,0,7,0q�5]   "   $��30��5^   "   ��5`      �� 5a   "   " ��6(   # [  5b   (   .�5c   &   ��  �@�Tag$	 False��5d   !   61   
 5e   &   �� 5f   *    66   �� *$"AppService.DIOModule1.IOMapping[3]+ True5g   6   .a�5h   .   ��  �@�Tag$
	 True��6D   5i   +   5k   #   5m      �� 5n      $
��Stop��6$   $��Center=�5o   $   $��13(�5p   $   5q      � 5r      $ 
Horizontal=�6    $	��0,5q�5s       $��Center=�5t       .+�5u      �� $��Right=�6"   $��0,0,7,0q�5v   "   $��30��5w   "   ��5x      �� 5y   "   " ��6(   # [  5z   (   .�5{   &   ��  �@�Tag$	 False��5|   -   6=   
 5}   &   �� 5~   *    66   �� *$"AppService.DIOModule1.IOMapping[2]+ True5   6   .a�5�   .   ��  �@�Tag$
	 True��5�   5   5�   +   5�   #   5�      �� 5�      $��Reset��6$   $��Center=�5�   $   $��13(�5�   $   5�      � 5�      $ 
Horizontal=�6    $	��0,5q�5�       $��Center=�5�       .+�5�      �� $��Right=�6"   $��0,0,7,0q�5�   "   $��30��5�   "   ��5�      �� 5�   "   " ��6(   # [  5�   (   .�5�   &   ��  �@�Tag$	 False��5�   -   6=   
 5�   &   �� 5�   *    66   �� *$"AppService.DIOModule2.IOMapping[0]+ True5�   6   .a�5�   .   ��  �@�Tag$
	 True��5�   5   5�   +   5�   #   5�      �� 5�      $��Service Key��6$   $��Center=�5�   $   $��13(�5�   $   5�      � 5�      $ 
Horizontal=�6    $	��0,5q�5�       $��Center=�5�       .+�5�      �� $��Right=�6"   $��0,0,7,0q�5�   "   $��30��5�   "   ��5�      �� 5�   "   " ��6(   # [  5�   (   .�5�   &   ��  �@�Tag$	 False��5�   -   6=   
 5�   &   �� 5�   *    66   �� *$"AppService.DIOModule1.IOMapping[1]+ True5�   6   .a�5�   .   ��  �@�Tag$
	 True��5�   5   5�   +   5�   #   5�      �� 5�      $	��EMO��6$   $��Center=�5�   $   $��13(�5�   $   5�      � 5�      $ 
Horizontal=�6    $	��0,5q�5�       $��Center=�5�       .+�5�      �� $��Right=�6"   $��0,0,7,0q�5�   "   $��30��5�   "   ��5�      �� 5�   "   " ��6(   # [  5�   (   .�5�   &   ��  �@�Tag$	 False��5�   -   6=   
 5�   &   �� 5�   *    66   �� *$"AppService.DIOModule2.IOMapping[1]+ True5�   6   .a�5�   .   ��  �@�Tag$
	 True��5�   5   5�   +   5�   #   5�      �� 5�      $��Z Stage Sensor��6$   $��Center=�5�   $   $��13(�5�   $   5�      � 5�      $ 
Horizontal=�6    $	��0,5q�5�       $��Center=�5�       .+�5�      �� $��Right=�6"   $��0,0,7,0q�5�   "   $��30��5�   "   ��5�      �� 5�   "   " ��6(   # [  5�   (   .�5�   &   ��  �@�Tag$	 False��5�   -   6=   
 5�   &   �� 5�   *    66   �� *$"AppService.DIOModule2.IOMapping[2]+ True5�   6   .a�5�   .   ��  �@�Tag$
	 True��5�   5   5�   +   5�   #   5�      �� 5�      $��Fixture Sensor��6$   $��Center=�5�   $   $��13(�5�   $   5�      5�      �� 5�      $ Vertical=�6   $��10,5,0,0q�5�      $��1��5�      .+�5�      � $ 
Horizontal=�6    $	��0,5q�5�       $��Center=�5�       .+�5�      �� $��Right=�6"   $��0,0,7,0q�5�   "   $��30��5�   "   ��5�      �� 5�   "   " ��6(   # [  5�   (   .�5�   &   ��  �@�Tag$	 False��5   -   6=   
 5  &   �� 5  *    66   �� *$"AppService.DIOModule2.IOMapping[3]+ True5  6   .a�5  .   ��  �@�Tag$
	 True��5  5   5  +   5  #   5
     �� 5     $��Left Door Sensor��6$   $��Center=�5  $   $��13(�5  $   5     � 5     $ 
Horizontal=�6    $	��0,5q�5      $��Center=�5      .+�5     �� $��Right=�6"   $��0,0,7,0q�5  "   $��30��5  "   ��5     �� 5  "   " ��6(   # [  5  (   .�5  &   ��  �@�Tag$	 False��5  -   6=   
 5  &   �� 5  *    66   �� *$"AppService.DIOModule2.IOMapping[4]+ True5  6   .a�5  .   ��  �@�Tag$
	 True��5  5   5  +   5!  #   5#     �� 5$     $��Right Door Sensor��6$   $��Center=�5%  $   $��13(�5&  $   5'     � 5(     $ 
Horizontal=�6    $	��0,5q�5)      $��Center=�5*      .+�5+     �� $��Right=�6"   $��0,0,7,0q�5,  "   $��30��5-  "   ��5.     �� 5/  "   " ��6(   # [  50  (   .�51  &   ��  �@�Tag$	 False��52  -   6=   
 53  &   �� 54  *    66   �� *$"AppService.DIOModule1.IOMapping[0]+ True55  6   .a�56  .   ��  �@�Tag$
	 True��57  5   58  +   5:  #   5<     �� 5=     $��Front Door Status��6$   $��Center=�5>  $   $��13(�5?  $   5@     � 5A     $ 
Horizontal=�6    $	��0,5q�5B      $��Center=�5C      .+�5D     �� $��Right=�6"   $��0,0,7,0q�5E  "   $��30��5F  "   ��5G     �� 5H  "   " ��6(   # [  5I  (   .�5J  &   ��  �@�Tag$	 False��5K  -   6=   
 5L  &   �� 5M  *    66   �� *$"AppService.DIOModule1.IOMapping[6]+ True5N  6   .a�5O  .   ��  �@�Tag$
	 True��5P  5   5Q  +   5S  #   5U     �� 5V     $��Main Air Status��6$   $��Center=�5W  $   $��13(�5X  $   5Y     � 5Z     $ 
Horizontal=�6    $	��0,5q�5[      $��Center=�5\      .+�5]     �� $��Right=�6"   $��0,0,7,0q�5^  "   $��30��5_  "   ��5`     �� 5a  "   " ��6(   # [  5b  (   .�5c  &   ��  �@�Tag$	 False��5d  -   
 5e  &   �� 5f  *    66   �� *$"AppService.DIOModule1.IOMapping[7]+ True5g  6   .a�5h  .   ��  �@�Tag$
	 True��5i  5   5j  +   5l  #   5n     �� 5o     $��Vaccum Button��6$   $��Center=�5p  $   $��13(�5q  $   5r     � 5s     $ 
Horizontal=�6    $	��0,5q�5t      $��Center=�5u      .+�5v     �� $��Right=�6"   $��0,0,7,0q�5w  "   $��30��5x  "   ��5y     �� 5z  "   " ��6(   # [  5{  (   .�5|  &   ��  �@�Tag$	 False��5}  -   
 5~  &   �� 5  *    66   �� *$"AppService.DIOModule2.IOMapping[5]+ True5�  6   .a�5�  .   ��  �@�Tag$
	 True��5�  5   5�  +   5�  #   5�     �� 5�     $��Safety Circuit��6$   $��Center=�5�  $   $��13(�5�  $   5�     5�     5�     � 5�     $��1��6   $��1��6     5�     �� 5�     $��1* �6'   �� 5�     $
��0.8* �6'   �� 5�     $��1* �6'    5�     �� 5�     $
��0.2* �6$   �� 5�     .+�5�     �� $��0��6   $��3��6&   $��7,0,7,5q�5�     
���iii�$��1,1,1,1q�5�     ._�5�     � $ 
Horizontal=�6    $��Center=�5�      $
��Left=�5�      .+�5�     ��   CustomButtonStyle#��[ 6!   $��	50,0,20,0q�5�  !   $�� Home��5�  !   $��Normal"�5�  !   $��12(�5�  !   $��35��5�  !   $��90��5�  !   $
��home��5�  !   ���#9FB6CD��5�  !   �� *AppCommand.cmdIAIAbsoluteMove+5�     �� 5�     #��[ 6!   $��0,0,20,0q�5�  !   $��Servo On��63   $��Normal"�5�  !   $��12(�5�  !   $��35��5�  !   $��90��5�  !   $
��SVON��5�  !   ���#9FB6CD��5�  !   �� *AppCommand.cmdIAIServoOnOff+5�  !   �� 5�     #��[ 6!   $��0,0,20,0q�5�  !   $��	Servo Off��63   $��Normal"�5�  !   $��12(�5�  !   $��35��5�  !   $��90��5�  !   
������ $��SVOFF��5�  !   ���#9FB6CD��5�  !   �� *AppCommand.cmdIAIServoOnOff+5�  !   �� 5�     #��[ 6!   $��0,0,40,0q�5�  !   $��Alarm Reset��63   $��Normal"�5�  !   $��12(�5�  !   $��35��5�  !   $��90��5�  !   
������ ���#9FB6CD��5�  !   �� *AppCommand.cmdIAIAlarmReset+5�  !   �� 5�     $ 
Horizontal=�6%   $��Center=�6>   .+�5�     � .+�5�  "   �� $��Axes X:��6(   $��Center=�6:   $��0,5,0,0q�6U   �� 5�  "   $��50��6+   $��40��66   $��Center=�5�  +   $��Center=�5�  +   $��5q�5�  +   $��XCheck��5�  +    d� 	IsChecked 5�  +   �� *&$AppService.IAIMotion.IsSelectedAxisX+��5�  +   �� *!AppCommand.cmdIAIToggleCheckBox+5�  #   5�     � 5�     $��20,0,0,0q�6(   .+�5�  "   �� $��Axes Y: ��6(   $��Center=�6;   6X   �� 5�  "   $��50��6+   $��40��66   $��Center=�5�  +   $��Center=�5�  +   $��5q�5�  +   $��YCheck��5�  +    5�  +   �� *&$AppService.IAIMotion.IsSelectedAxisY+��5�  +   �� *!AppCommand.cmdIAIToggleCheckBox+5�  #   5�     5�     5�     5�     �� 5�     $��0��6   $��1��6)   $��7,0,7,5q�5�     
���iii�$��1,1,1,1q�5�     ._�5�     � $ Vertical=�6    $��Center=�67   .+�5�     �� .+�5�     �� $��Center=�6)   $ 
Horizontal=�5�  )   $��Center=�5�  )   .+�5�  "   � $��1��6,   $
��Left=�5�  (   $��Center=�5�  (   .+�5�  &   �� $��● Speed(mm/s):��6,   $��	10,20,0,0q�6E   $��Normal"�5�  (   $��12(�5�  (   $��45��5�  (   �� 5�  &   -   5         ��Name$ inputJogSpeed��5�  .   $��Center=�6E   $��50��5�  *   $��Center=�65   5�  #   �� 5�  "   $��Apply��6)   #��[ 5�  %   $��10q�5�  %   $��Normal"�5�  %   $��12(�5�  %   $��90��63   $��35��6>   $��Right=�5�  %   
������ $��Bottom=�5�  %   ���#9FB6CD��5�  %   �� *!AppCommand.cmdIAIUpdateJogSpeed+��5�  %   ��  �� ElementName$ inputJogSpeed�� �� Path$
 Text�5�  %   5�     � 5�     $��1��6(   $��0,30,0,0q�65   $��1��5�  $   $��Center=�5   $   $��Center=�5  $   $ 
Horizontal=�5  $   .+�5  "   �� #��[ 6)   $�� JOG X - ◀��5  %   $��Normal"�5  %   $��12(�5  %   $��0 0 20 0q�63   $��90��5  )   $��35��64   
������ ���#9FB6CD*  $Microsoft.Xaml.Behaviors.Interaction   Triggers 5
  &   +  %Microsoft.Xaml.Behaviors.EventTrigger  5  *      	EventName$ PreviewMouseDown��69   *  $Microsoft.Xaml.Behaviors.TriggerBase   Actions. 5  .   -  'Prism.Interactivity.InvokeCommandAction     CommandParameter$ Press��6y      Command 6H   �� *AppCommand.cmdIAIJogBackward+6y   5  +     5  *   $ PreviewMouseUp��69   . 5  .     $ Release��6y    6H   �� *AppCommand.cmdIAIJogBackward+6y   5  +   5  #   �� 5  "   #��[ 6)   $��▶ JOG X +��5  %   $��Normal"�5  %   $��12(�5  %   $��0q�63   $��90��5  )   $��35��64   
������ ���#9FB6CD 5  &     5  *   $ PreviewMouseDown��69   . 5  .     $ Press��6x    6H   �� *AppCommand.cmdIAIJogForward+6x   5  +     5  *   $ PreviewMouseUp��69   . 5  .     $ Release��6x    6H   �� *AppCommand.cmdIAIJogForward+6x   5   +   5"  #   5#     � 5$     $��1��6(   $��0,30,0,0q�65   $��1��5%  $   $��Center=�5&  $   $��Center=�5'  $   $ 
Horizontal=�5(  $   .+�5)  "   �� #��[ 6)   $�� JOG Y - ◀��5*  %   $��Normal"�5+  %   $��12(�5,  %   $��90��63   $��35��6>   $��0 0 20 0q�5-  )   
������ ���#9FB6CD 50  &     51  *   $ PreviewMouseDown��69   . 52  .     $ Press��6~    6H   �� *#!AppCommand.cmdIAIAxisYJogBackward+6~   53  +     54  *   $ PreviewMouseUp��69   . 55  .     $ Release��6~    6H   �� *#!AppCommand.cmdIAIAxisYJogBackward+6~   56  +   58  #   �� 59  "   #��[ 6)   $��▶ JOG Y +��5:  %   $��Normal"�5;  %   $��12(�5<  %   $��90��5=  %   $��35��60   
������ ���#9FB6CD 5@  &     5A  *   $ PreviewMouseDown��69   . 5B  .     $ Press��6}    6H   �� *" AppCommand.cmdIAIAxisYJogForward+6}   5C  +     5D  *   $ PreviewMouseUp��69   . 5E  .     $ Release��6}    6H   �� *" AppCommand.cmdIAIAxisYJogForward+6}   5F  +   5H  #   5I     5J     5K     5L     �� 5M     $��1��6   $��1��6)   
���iii�$��0 0 5 0q�5N  +   $��1,1,1,1q�5O     ._�5P     �� $	��Top=�6!   $��Center=�5Q  !   .+�5R     �� $��Center=�6%   $��Center=�6@   .+�5S     � $��Center=�6(   $��Center=�6C   .+�5T  "   �� $"��Axis X Moving Position (mm):��6(   $��Center=�5U  (   �� 5V  "   $��5q�6[   $��Center=�6f   $��Center=�5W  *   $��Center=�6E   $	��100��6b   ��5V  *   �� *!AppService.IAIMotion.AxisXValue+5W  b   5X     � 5Y     $��Center=�6(   $��Center=�6C   .+�5Z  "   �� $"��Axis Y Moving Position (mm):��6(   $��Center=�5[  (   �� 5\  "   $��5q�6[   $��Center=�6f   $��Center=�5]  *   $��Center=�6E   $	��100��6b   ��5\  *   �� *!AppService.IAIMotion.AxisYValue+5]  p   5^     � 5_     5`     5a     � 5b     $ 
Horizontal=�6$   $
��Left=�6=   $��0 0 0 5q�5c  $   .+�5d     �� #��[ 6%   $��	0,10,10,0q�5e  %   $��Relative Move��5f  %   $��Normal"�5g  %   $��12(�5h  %   $��35��5i  %   $��90��61   
������ ���#9FB6CD��5k  %   �� *AppCommand.cmdIAIRelativeMove+5l  %   �� 5m     #��[ 6%   $��Absolute Move��5n  %   $��Normal"�5o  %   $��12(�5p  %   $��0 10 0 0q�63   $��35��5q  %   $��90��5r  %   
������ $	��ABS��5u  %   ���#9FB6CD��5t  %   �� *AppCommand.cmdIAIAbsoluteMove+5v  %   5w     � 5x     $ 
Horizontal=�6$   $
��Left=�6=   $��0 0 0 5q�5y  $   .+�5�     �� #��[ 6%   $��	0,10,10,0q�5�  %   $��Normal"�5�  %   $��12(�5�  %   $��35��5�  %   $��90��5�  %   
������ $	��ABS��5�  %   ���#9FB6CD��5�  %   �� *" AppCommand.cmdIAITestPatternMove+.��5�  "   �� $��Test Pattern Move��6,   $
��Wrap=�6E    �� TextAlignment$ Center=�6Y   5�     �� 5�     #��[ 6%   $��Normal"�5�  %   $��12(�5�  %   $��0 10 0 0q�63   $��35��5�  %   $��90��5�  %   
������ $	��ABS��5�  %   ���#9FB6CD��5�  %   �� *" AppCommand.cmdIAIStopPatternMove+.��5�  "   �� $��Stop Program Move��6,   $
��Wrap=�6E   $ Center=�6Y   5�     5�     5�     5�     �� 5�     $��2��6   $��1��6)   
���iii�$��1,1,1,1q�5�     ._�5�     �� $��Axes Status��6   ���	#FF747474$��1q�5�     .��5�     � $ 
Horizontal=�6$   $��Center=�6=   $
��Left=�6X   .+�5�     � $ Vertical=�6(   $
��Left=�6?   $��0 0 3 0q�5�  (   .+�5�  "   �� $
��Bold"�6(   $��Italic$�6:   $
��Axis��5�     $��Center=�6+   $��Center=�5�     �� 5�  "   $��25��6(   $��30��63   $
��Bold"�6?   $��Italic$�6Q   $��X��5�     $
��Left=�6(   $��Center=�5�     �� 5�  "   $��25��6(   $��30��63   $
��Bold"�6?   $��Italic$�6Q   $��Y��5�     $
��Left=�6(   $��Center=�5�     5�     � 5�     $ Vertical=�6(   $
��Left=�6?   $��0 0 3 0q�5�  (   .+�5�  "   �� $
��Bold"�6(   $��Italic$�6:   $��Coordinates��5�     $��Center=�62   $��Center=�5�     �� 5�  "   $��80��6(   $��30��63   $
��Bold"�6?   $��Italic$�6Q   $��Center=�5�  Q   $��Center=�5�     ��5�     �� *" AppService.IAIMotion.XCoordinate+5�     �� 5�  "   $��80��6(   $��30��63   $
��Bold"�6?   $��Italic$�6Q   $��Center=�5�  Q   $��Center=�5�     ��5�     �� *" AppService.IAIMotion.YCoordinate+5�     5�     � 5�     $ Vertical=�6(   $
��Left=�6?   $��0 0 3 0q�5�  (   .+�5�  "   �� $
��Bold"�6(   $��Italic$�6:   $
��Busy��5�     $��Center=�6+   $��Center=�5�     �� 5�  "   $��30��6(   $
��Bold"�64   $��Italic$�6F   $��Center=�5�  K   $��Center=�5�     ��5�     �� *AppService.IAIMotion.XBusy+5�     �� 5�  "   $��30��6(   $
��Bold"�64   $��Italic$�6F   $��Center=�5�  K   $��Center=�5�     ��5�     �� *AppService.IAIMotion.YBusy+5�     5�     � 5�     $ Vertical=�6(   $
��Left=�6?   $��0 0 3 0q�5�  (   .+�5�  "   �� $
��Bold"�6(   $��Italic$�6:   $��Finish��5�     $��Center=�6-   $��Center=�5�     �� 5�  "   $��30��6(   $
��Bold"�64   $��Italic$�6F   $��Center=�5�  M   $��Center=�5�     ��5�     �� *AppService.IAIMotion.XFinish+5�     �� 5�  "   $��30��6(   $
��Bold"�64   $��Italic$�6F   $��Center=�5�  M   $��Center=�5�     ��5�     �� *AppService.IAIMotion.YFinish+5�     5�     � 5�     $ Vertical=�6(   $
��Left=�6?   .+�5�  "   �� $
��Bold"�6(   $��Italic$�6:   $��Error��5�     $��Center=�6,   $��Center=�5�     �� 5�  "   $��30��6(   $
��Bold"�64   $��Italic$�6F   $��Center=�5�  L   $��Center=�5�     ��5�     �� *AppService.IAIMotion.XError+5�     �� 5�  "   $��30��6(   $
��Bold"�64   $��Italic$�6F   $��Center=�5�  L   $��Center=�5�     ��5�     �� *AppService.IAIMotion.YError+5�     5�     5�     5�     5�     5�     �� 5�     $��1��6   $��2��6)   $��7,0,7,5q�5�     
���iii�$��0,0,0,1q�5�     ._�5�     �� $
��Left=�6   $��Center=�5�     $
��Left=�5�     $	��200��5�     
������ 
���   �$��17(�5�     $��Normal"�5�     $�� Output��5�     62   5      �� 5     $��1��6   $��2��6%   $��7,0,7,5q�5     
���iii�$��0,0,0,1q�5     ._�5     �� $
��Left=�6   $��Center=�5     $
��Left=�5     $	��200��5     
������ 
���   �$��17(�5     $��Normal"�5     $�� Precitec Controller��5     5     � 5     $��1��6   $��3��6#   .+�5     ��  �� SelectedIndex 6   �� *PrecitecTabSelect+.@�5     �� $��	1D Sensor��6   y�61   �� *Tab1D+.��5     �  5     �� 5  "   $
��333* �63   �� 5  "   $
��217* �63   �� 5  "   $
��116* �63   .+�5     �� $��1D Sensor Control��6'   $��13(�6B   $��5,0,5,5q�5  '   $��2q�68   $��0��6L     MaterialDesignGroupBox#���  5  '   .  (MaterialDesignThemes.Wpf.ColorZoneAssist   Mode$ Custom=�5  '      
Background
 �iii�   
Foreground
 �����.��5  "   � $ Vertical=�6,   .+�5  &   � $ 
Horizontal=�60   .+�5   *   �� $��0��64   $��   ● Start Pos:��6A   $��Center=�6X   �� 5!  *   $	��5 0q�6k   $��Center=�6x   $��Center=�5"  2   $��Center=�6M   $��80��6j   ��5!  2   �� *)'AppService.PrecitecService.OneDStartPos+5"  j   5#  '   � 5$  &   $ 
Horizontal=�60   .+�5%  *   �� $��0��64   $��   ● Stop Pos:��6A   $��Center=�6W   �� 5&  *   $	��5 0q�6j   $��Center=�6w   $��Center=�5'  2   $��Center=�6M   $��80��6j   ��5&  2   �� *(&AppService.PrecitecService.OneDStopPos+5'  j   5(  '   � 5)  &   $ 
Horizontal=�60   .+�5*  *   �� $��0��64   $��   ● Interval:��6A   $��Center=�6W   �� 5+  *   $	��5 0q�6k   $��Center=�6x   $��Center=�5,  2   $��Center=�6M   $��80��6j   ��5+  2   �� *)'AppService.PrecitecService.OneDInterval+5,  j   5-  '   � 5.  &   $ 
Horizontal=�60   .+�5/  *   �� $��0��64   $#��   ● Number of scan linear:��6A   $��Center=�6d   �� 50  *   $	��5 0q�6m   $��Center=�6z   $��Center=�51  2   $��Center=�6M   $��80��6j   ��50  2   �� *+)AppService.PrecitecService.OneDScanLineNo+51  j   52  '   � 53  &   $ 
Horizontal=�60   .+�54  *   �� $��0��64   $%��   ● Sample Counter Per Line:��6A   $��Center=�6f   �� 55  *   $	��5 0q�6u   $��Center=�6�   $��Center=�56  2   $��Center=�6M   $��80��6j    �� 
IsReadOnly . ��55  2   �� *31AppService.PrecitecService.OneDSampleCountPerLine+56  u   57  '   � 58  &   $ 
Horizontal=�60   .+�59  *   �� $��0��64   $#��   ● Device Output Signals:��6A   $��Center=�6d   �� 5:  *   $	��5 0q�6o   $��Center=�6|   $��Center=�5;  2   $��Center=�6M   $	��120��6j   ��5:  2   �� *-+AppService.PrecitecService.OneDSignalOutput+5;  j   5<  '   � 5=  &   $ 
Horizontal=�60   .+�5>  *   �� #��[ 61   $��5q�5?  1   $��Set ENC X Pos��5@  1   $��Normal"�5A  1   $��12(�5B  1   $��35��5C  1   $��90��6=   
������ ���#9FB6CD$��x��5G  1   ��5F  1   �� *AppCommand.cmdOneDSetEncPos+5G  1   �� 5H  *   $	��5 0q�6l   $��Center=�6y   $��Center=�5I  2   $��Center=�6M   $��80��6j   ��5H  2   �� **(AppService.PrecitecService.OneDValueEncX+5I  j   5J  '   � 5K  &   $ 
Horizontal=�60   .+�5L  *   �� #��[ 61   $��5q�5M  1   $��Set ENC Y Pos��5N  1   $��Normal"�5O  1   $��12(�5P  1   $��35��5Q  1   $��90��6=   
������ ���#9FB6CD$��y��5U  1   ��5T  1   �� *AppCommand.cmdOneDSetEncPos+5U  1   �� 5V  *   $	��5 0q�6l   $��Center=�6y   $��Center=�5W  2   $��Center=�6M   $��80��6j   ��5V  2   �� **(AppService.PrecitecService.OneDValueEncY+5W  j   5X  '   � 5Y  &   $ 
Horizontal=�60   $��Center=�6I   .+�5Z  *   �� #��[ 61   $��5q�5[  1   $��Normal"�5\  1   $��12(�5]  1   $��35��5^  1   $��90��6=   
������ ���#9FB6CD$��x��5b  1   ��5a  1   �� *AppCommand.cmdOneDSendSetting+.��5c  .   �� $��Send Enc X Setting��68   $ Center=�6R   $
��Wrap=�5d  8   5e  +   �� 5f  *   #��[ 61   $��5q�5g  1   $��Normal"�5h  1   $��12(�5i  1   $��35��5j  1   $��90��6=   
������ ���#9FB6CD$��y��5n  1   ��5m  1   �� *AppCommand.cmdOneDSendSetting+.��5o  .   �� $��Send Enc Y Setting��68   $ Center=�6R   $
��Wrap=�5p  8   5q  +   5r  '   5s  #   5t     �� 5u     $��Pattern Moving Setting��6'   $��13(�6G   $��5,0,5,5q�5v  '   $��2q�68   $��1��6L   #���  5w  '   $ Custom=�5x  '   
 �iii�
 �����$��2��5z  ^   .��5{  "   � $ Vertical=�6,   .+�5|  &   � $ 
Horizontal=�60   .+�5}  *   �� $��0��64   $��   ● Start Pos X1:��6A   $��Center=�6[   �� 5~  *   $	��5 0q�6]   $��Center=�6j   $��Center=�5  2   $��Center=�6M   $��80��6j   ��5~  2   �� *AppService.IAIMotion.PmX1+5  j   5�  '   � 5�  &   $ 
Horizontal=�60   .+�5�  *   �� $��0��64   $��   ● Start Pos Y1:��6A   $��Center=�6[   �� 5�  *   $	��5 0q�6]   $��Center=�6j   $��Center=�5�  2   $��Center=�6M   $��80��6j   ��5�  2   �� *AppService.IAIMotion.PmY1+5�  j   5�  '   � 5�  &   $ 
Horizontal=�60   .+�5�  *   �� $��0��64   $��   ● Width DX:��6A   $��Center=�6W   �� 5�  *   $	��5 0q�6b   $��Center=�6o   $��Center=�5�  2   $��Center=�6M   $��80��6j   ��5�  2   �� * AppService.IAIMotion.PmWidthDx+5�  j   5�  '   � 5�  &   $ 
Horizontal=�60   .+�5�  *   �� $��0��64   $��   ● Height DY:��6A   $��Center=�6X   �� 5�  *   $	��5 0q�6c   $��Center=�6p   $��Center=�5�  2   $��Center=�6M   $��80��6j   ��5�  2   �� *!AppService.IAIMotion.PmHeightDy+5�  j   5�  '   � 5�  &   $ 
Horizontal=�60   .+�5�  *   �� $��0��64   $��   ● Pitch Y:��6A   $��Center=�6V   �� 5�  *   $	��5 0q�6a   $��Center=�6n   $��Center=�5�  2   $��Center=�6M   $��80��6j   ��5�  2   �� *AppService.IAIMotion.PmPitchy+5�  j   5�  '   � 5�  &   $ 
Horizontal=�60   .+�5�  *   �� $��0��64   $��   ● Speed:��6A   $��Center=�6T   �� 5�  *   $	��5 0q�6`   $��Center=�6m   $��Center=�5�  2   $��Center=�6M   $��80��6j   ��5�  2   �� *AppService.IAIMotion.PmSpeed+5�  j   5�  '   � 5�  &   $ 
Horizontal=�60   .+�5�  *   �� $��0��64   $��   ● Acc:��6A   $��Center=�6R   �� 5�  *   $	��5 0q�6^   $��Center=�6k   $��Center=�5�  2   $��Center=�6M   $��80��6j   ��5�  2   �� *AppService.IAIMotion.PmAcc+5�  j   5�  '   � 5�  &   $ 
Horizontal=�60   .+�5�  *   �� $��0��64   $��   ● Dcl:��6A   $��Center=�6R   �� 5�  *   $	��5 0q�6^   $��Center=�6k   $��Center=�5�  2   $��Center=�6M   $��80��6j   ��5�  2   �� *AppService.IAIMotion.PmDcl+5�  j   5�  '   � 5�  &   $ 
Horizontal=�60   $��Center=�6I   .+�5�  *   �� #��[ 61   $��5q�5�  1   $��
Start Scan��5�  1   $��Normal"�5�  1   $��12(�5�  1   $��35��5�  1   $��90��6=   
������ ���#9FB6CD$��start��5�  1   ��5�  1   �� *AppCommand.cmdOneDStartScan+5�  1   �� 5�  *   #��[ 61   $��5q�5�  1   $��	Stop Scan��5�  1   $��Normal"�5�  1   $��12(�5�  1   $��35��5�  1   $��90��6=   
������ ���#9FB6CD$
��stop��5�  1   ��5�  1   �� *AppCommand.cmdOneDStartScan+5�  1   5�  '   5�  #   5�     5�     5�     �� 5�     $��
FSS Sensor��6   y�62   �� *TabFss+.��5�     �� $��fss here��6$   5�     �� 5�     $��Send Command��6   y�64   �� *
TabSendCmd+.��5�     � $ Vertical=�6$   .+�5�     � $ 
Horizontal=�6(   $��	10 10 0 0q�6A   .+�5�  "   �� $��0��6,   $��Send Command��69   $��10 0 0 0q�5�  ,   $��Center=�6>   �� 5�  "   #��[ 6)   $
��Send��5�  )   $��	10,0,10,0q�5�  )   $��Normal"�5�  )   $��12(�5�  )   $��90��5�  )   $��35��64   $��Right=�5�  )   
������ ���#9FB6CD��5�  )   �� *PrecitecSendCmd+5�  #   5�     �� 5�      ! MaterialDesignOutlinedTextBox#��[ 6&   $��10q�5�  ?   $	��Top=�5�  %   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Input��5�  %    �� AcceptsReturn . " �� VerticalScrollBarVisibility$
 Auto=�5�  %   ��5�  %   �� *
CmdInput+5�  %   � 5�     $ Vertical=�6(   .+�5�  "   �� $��Received Response��6,   $��10 0 0 0q�5�  ,   $��Center=�6>   �� 5�  "   $��5,0,5,5q�6+   $��1��6=   $	��250��6J   $��Stretch=�5�  +   $��Stretch=�6G   ��5�  +   �� *AppService.LogRspPrecitec+ 5�  +   �� *'%AppService.ScrollViewerVerticalOffset+ �� View 5�  &   �� 5�  *   .P�5�  .   ��   �� DisplayMemberBinding  6=   �� *
DateTime+��5�  2   �� 5�  6   $
��Time��6@   $ Center=�6L   5�  /   �� 5�  .   ! �� Width$	! 140��6>     6J   �� *
strLogType+��5�  2   �� 5�  6   $��Log Type��6@   $ Center=�6P   5�  /   �� 5�  .     6>   �� *
strContent+��5�  2   �� 5�  6   $��Description��6@   $ Center=�6S   $��NoWrap=�6j   5�  /   5�  +   5�  #   5�     5�     5�     5�     5�     �� 5�     $��1��6   $��1��6)   $��7,0,7,5q�5�     
���iii�$��0,1,1,1q�5�     5�     �� 5�     $��3��6   
�������$��0,0,1,0q�5�     61   � 5�     $��3��6    5�     �� 5�     $��* �6'   63   �� 5�     $��2* �6'   64    5�     �� 5�     $��* �6$   61   �� 5�     $��* �6$   61   �� 5�     $��* �6$   61   �� 5�     $��* �6$   61   �� 5�     $��* �6$   61   .+�5�     �� $��0��6   $��   ● Vacuum��6)   $��Center=�6<   �� 5�     $��1��6   $��   ● Release Vaccum��5�     $��Center=�67   �� 5�     $��2��6   $V��P   ● Power For 1D/FSS                                       (ON: FSS, OFF: 1D)��5�     $
��Wrap=�5�     $��Center=�5�     �� 5�     $��3��6   $��   ● Maitenance Light��5�     $��Center=�5�     � 5�     $��1��6   $��0��6-   $ 
Horizontal=�6:   $��Center=�5�     $��Center=�69   .+�5�     �� #��[ 6   $��ON��5�     $��VacOn��5�     $��0,0,10,0q�5�     $��Normal"�5�     $��12(�5�     $��90��5�     $��35��6(   $��Right=�5�     
������ ���#9FB6CD��5�     �� *AppCommand.cmdDOVaccumOnOff+5�     �� 5�     $��Right=�6   $��0,0,10,0q�5�     $��30��5      ��5     �� 5     " ��6$   # [  5  $   .�5  "   ��  �@�Tag$	 False��5  )   
 5  "   �� 5  &    62   �� *$"AppService.DIOModule1.IOMapping[5]+ True5  2   .a�5	  *   ��  �@�Tag$
	 True��5
  1   5  '   5     5     �� 5     #��[ 6   $	��OFF��5     $��VacOff��5     $��Normal"�5     $��12(�5     $��90��5     $��35��6(   $��Right=�5     
������ ���#9FB6CD��5     �� *AppCommand.cmdDOVaccumOnOff+5     5     � 5     $��1��6   $��1��5     $ 
Horizontal=�5     $��Center=�5     $��Center=�5     .+�5      �� -   5        #��[ 5      $��ON��5!     $��0,0,10,0q�5#     $��Normal"�5$     $��12(�5%     $��90��5&     $��35��6(   $��Right=�5'     
������ ���#9FB6CD��5"     �� *	TestEvent+5)  I   �� 5*     $��Right=�6   $��0,0,10,0q�5+     $��30��5,     ��5-     �� 5.     " ��6$   # [  5/  $   .�50  "   ��  �@�Tag$	 False��51  )   69   
 52  "   �� 53  &    62   ��  True54  2   .a�55  *   ��  �@�Tag$
	 True��56  1   57  '   59     5;     �� 5<     #��[ 6   $	��OFF��5=     $��BlowOff��5?     $��Normal"�5@     $��12(�5A     $��90��5B     $��35��6(   $��Right=�5C     
������ ���#9FB6CD��5>     �� *ReleaseVaccumOnOffCmd+5E     5F     � 5G     $��1��6   $��2��5H     $ 
Horizontal=�5I     $��Center=�5J     $��Center=�5K     .+�5L     �� #��[ 6   $��ON��5M     $	��FSS��5O     $��0,0,10,0q�5P     $��Normal"�5Q     $��12(�5R     $��90��5S     $��35��6(   $��Right=�5T     
������ ���#9FB6CD��5N     �� *#!AppCommand.CmdDOPowerSupplySensor+5V     �� 5W     $��Right=�6   $��0,0,10,0q�5X     $��30��6/   ��5Y     �� 5Z     " ��6$   # [  5[  $   .�5\  "   ��  �@�Tag$	 False��5]  )   
 5^  "   �� 5_  &    62   ��  True5`  2   .a�5a  *   ��  �@�Tag$
	 True��5b  1   5c  '   5e     5g     �� 5h     #��[ 6   $	��OFF��5i     $��1D��5k     $��Normal"�5l     $��12(�5m     $��90��5n     $��35��6(   $��Right=�5o     
������ ���#9FB6CD��5j     �� *#!AppCommand.CmdDOPowerSupplySensor+5q     5r     � 5s     $��1��6   $��3��5t     $ 
Horizontal=�5u     $��Center=�5v     $��Center=�5w     .+�5x     �� #��[ 6   $��ON��5y     $	��LOn��5{     $��0,0,10,0q�5|     $��Normal"�5}     $��12(�5~     $��90��5     $��35��6(   $��Right=�5�     
������ ���#9FB6CD��5z     �� *AppCommand.cmdDOLightOnOff+5�     �� 5�     $��Right=�6   $��0,0,10,0q�5�     $��30��5�     ��5�     �� 5�     " ��6$   # [  5�     .�5�  "   ��  �@�Tag$	 False��5�  !   61   
 5�  "   �� 5�  &    62   ��  True5�  *   .a�5�  *   ��  �@�Tag$
	 True��5�  )   5�  '   5�     5�     �� 5�     #��[ 6   $	��OFF��5�     $
��LOff��5�     $��Normal"�5�     $��12(�5�     $��90��5�     $��35��6(   $��Right=�5�     
������ ���#9FB6CD��5�     �� *AppCommand.cmdDOLightOnOff+5�     5�     5�     5�     5�     5�     