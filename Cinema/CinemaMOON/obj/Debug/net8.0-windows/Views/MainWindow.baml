   M S B A M L   `   `   `  ����E  ACinemaMOON, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:CinemaMOON.ConvertersCinemaMOON.Converters  5      3clr-namespace:CinemaMOON.ViewsCinemaMOON.Views  5      =#clr-namespace:CinemaMOON.ViewModelsCinemaMOON.ViewModels  5
      !    CinemaMOON.Views.MainWindow   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      :
converters#clr-namespace:CinemaMOON.Converters     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5      *localclr-namespace:CinemaMOON.Views  5      Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006  5	      ,vm#clr-namespace:CinemaMOON.ViewModels  5
         WindowTitle  � Title#  �   5      $
��1200��5      $	��900��5      $	��800��5      $	��600��5       � AllowsTransparency$
 True��5      
���   � � WindowStartupLocation$ CenterScreen=�5       � WindowStyle$
 None=�5      '  !System.Windows.Shell.WindowChrome   WindowChrome 5        5   
      CaptionHeight$ 0Q�5         CornerRadius$ 0��5         GlassFrameThickness$ 0q�5         UseAeroCaptionButtons$ False��5      	 � 	Resources	 5      8   2CinemaMOON.Converters.BooleanToVisibilityConverter   BooleanToVisibilityConverter  TitleBarButtonStyle
 �� 
TargetType �� Property �� Value �� 
TargetType ��Name �� Triggers P� Property P� Value �� 
TargetName  CloseButtonStyle �� BasedOn �� Triggers  ErrorTextStyle  TransparentButtonStyle �� CornerRadius  GoldButtonStyle  LangButtonStyle  LoginLangButtonStyle��%  &	       &	      &	 5    7  '�� �    'G� -    &	 �
    &	 �    &	 l    &	 �    &	     7    5   
   62   �� 5"   
   "
 ��6,   .�5#      ��  � ��$ 30��6&   �� 5$       � ��$ 40��6'   �� 5%       � ��$	 Top=�62   �� 5&       � ��
 ���� 6+   �� 5'       � ��
 ���� 6,   �� 5(       � ��$ 0q�60   �� 5)       � ��
 �����6+   �� 5*       � �� 5+      �� 5,      " ��6&   .R�5-      �� $ bdr_main��5.      #��z��5/      #��z��50      #��z��51      ._�52      �� $��Center=�53   !   $��Center=�54   !   $��Content��55   !   56       57      P� 58       �@J�IsMouseOver$
 True��6=   . �59   "   �� $ bdr_main��6)    � �� �#4747476U   5:      P� 5;       � ��$
 True��6;   . �5<   "   �� $ bdr_main��6)    � �� �#6060606U   5=      5?      5A      5B      �� 5D   
   8   "
 ��5G      .�5H      ��  � �� 5I      �� 5J      " ��6&   .R�5K      �� $ bdr_main��5L      #��z��5M      #��z��5N      #��z��5O      ._�5P      �� $��Center=�5Q   !   $��Center=�5R   !   $��Content��5S   !   5T       5U      P� 5V       �@J�IsMouseOver$
 True��6=   . �5W   "   �� $ bdr_main��6)    � �� �#FF00006U   �� 5X   "    � ��
 �����6?   5Y      P� 5Z       � ��$
 True��6;   . �5[   "   �� $ bdr_main��6)    � �� �#CC00006U   �� 5\   "    � ��
 �����6?   5]      5_      5a      5b      �� 5d   
   "
 ��6   .�5e      ��  � ��
 �����6+   �� 5f       � ��
 ���� 6+   �� 5g       � �� �#D7B4726,   �� 5h       � ��$ 0,0,0,1q�60   �� 5i       � ��$ 5q�6(   �� 5j       � ��$ 16(�6)   �� 5k       �@��
CaretBrush �#D7B4726+   �� 5l       �@��SelectionBrush �	#A0D7B4726/    5m      P� 5n       �@J�IsMouseOver$
 True��61   . �5o      ��  �@�Cursor$ IBeam��6/   �� 5p       � �� �#FFD70064   5q      P� 5r       �@J�	IsFocused$
 True��6/   . �5s      ��  � �� �#FFD70064   5t      5v      �� 5x   
   "
 G�6   .�5y      ��  � ��
 �����6+   �� 5z       � ��
 ���� 6+   �� 5{       � �� �#D7B4726,   �� 5|       � ��$ 0,0,0,1q�60   �� 5}       � ��$ 5q�6(   �� 5~       � ��$ 16(�6)   �� 5       �@G�
CaretBrush �#D7B4726+   �� 5�       �@G�PasswordChar$	 ●��6-   �� 5�       �@G�SelectionBrush �	#A0D7B4726/    5�      P� 5�       �@J�IsMouseOver$
 True��61   . �5�      ��  �@�Cursor$ IBeam��6/   �� 5�       � �� �#FFD70064   5�      P� 5�       �@J�	IsFocused$
 True��6/   . �5�      ��  � �� �#FFD70064   5�      5�      �� 5�   
   "
 ��6'   .�5�      ��  � �� �#FF6B6B6+   �� 5�       � ��$ 12(�6)   �� 5�       � ��$ 0,2,0,5q�6'   �� 5�       � ��$
 Wrap=�6-   5�      �� 5�   
   "
 ��6/   .�5�      ��  � ��
 ���� 6+   �� 5�       � �� �#D7B4726,   �� 5�       � �� �#D7B4726+   �� 5�       � ��$ 1q�60   �� 5�       � ��$
 10,5q�6(   �� 5�       � ��$ 12(�6)   �� 5�       � ��$ SemiBold"�6+   �� 5�       �@�Cursor$
 Hand��6'   �� 5�       � �� 5�      �� 5�      " ��6&   .R�5�      �� $ PART_Border��5�      #��z��5�      #��z��5�      #��z��5�      $ 5��5�      ._�5�      �� #��z��5�   !   #��z��5�   !   #��z��5�   !   5�       5�      P� 5�       �@J�IsMouseOver$
 True��6=   . �5�   "   �� $ PART_Border��6)    � �� �	#20D7B4726X   5�      P� 5�       � ��$
 True��6;   . �5�   "   �� $ PART_Border��6)    � �� �	#40D7B4726X   5�      P� 5�       � {�$ False��6;   . �5�   "   ��  �@J�Opacity$	 0.6Q�6<   5�      5�      5�      5�      �� 5�   
   "
 ��6(   .�5�      ��  � �� �#D7B4726+   �� 5�       � �� �#D7B4726,   �� 5�       � ��
 �   �6+   �� 5�       � ��$ 1q�60   �� 5�       � ��$
 10,5q�6(   �� 5�       � ��$ 12(�6)   �� 5�       � ��$ SemiBold"�6+   �� 5�       �@�Cursor$
 Hand��6'   �� 5�       � �� 5�      �� 5�      " ��6&   .R�5�      �� $ PART_Border��5�      #��z��5�      #��z��5�      #��z��5�      $ 5��5�      ._�5�      �� #��z��5�   !   #��z��5�   !   #��z��5�   !   5�       5�      P� 5�       �@J�IsMouseOver$
 True��6=   . �5�   "   �� $ PART_Border��6)    � �� �#FFD7006X   �� 5�   "   $ PART_Border��6)    � �� �#FFD7006Y   5�      P� 5�       � ��$
 True��6;   . �5�   "   �� $ PART_Border��6)    � �� �#C6A35D6X   �� 5�   "   $ PART_Border��6)    � �� �#C6A35D6Y   5�      P� 5�       � {�$ False��6;   . �5�   "   ��  �@J�Opacity$	 0.6Q�6<   5�      5�      5�      5�      �� 5�   
   "
 ��6(   .�5�      ��  � ��
 ���� 6+   �� 5�       � ��$ 0q�60   �� 5�       � ��$ 40��6&   �� 5�       � ��$ 40��6'   �� 5�       �@�Cursor$
 Hand��6'   �� 5�       � �� 5�      �� 5�      " ��6&   .R�5�      �� #��z��6!   $ 3��6K   ._�5�      �� $��Center=�6/   $��Center=�6L   5�       5�      P� 5�       �@J�IsMouseOver$
 True��6=   . �5�   "   ��  � �� �	#33FFFFFF6?   5�      P� 5�       � ��$
 True��6;   . �5�   "   ��  � �� �	#66FFFFFF6?   5�      5�      5�      5�      �� 5�   
   8   "
 ��5      .�5     ��  � ��$ Center=�6;   �� 5      � ��$ Center=�69   5     5     .��5     �  � RowDefinitions 5  
   �� 5	     $��40 �6   �� 5
     $��1 �6   �� 5     $��* �6   .+�5  
   � -   5        $��0��5     
���   �.+�5     � $
��Left=�6   $��Center=�63   .+�5     �� $��30��5     $��5,0,0,0q�5     $��Center=�5     $,��&/Resources/Images/IconForCinemaApp.png��5     �� 5     $��10,0,0,0q�5     $��Center=�5     $��18(�5     ���#D7B472#���   5     5     �� 5     $��Right=�6    �� Orientation$ 
Horizontal=�65   .+�5      �� -   5         ��Name$ buttonMinimise��5!     #��[ 5#     .��5$     �� $��18��5%     $��18��5&     $*��$/Resources/Images/MinimizeWindow.png��5'     5(     �� 5)     -   5        $ buttonMaximise��5*     #��[ 5,     .��5-     �� -   5         ��Name$ maximizeImage��5.     $��20��5/     $��20��50     $*��$/Resources/Images/MaximizeWindow.png��51     52     �� 53     -   5        $ buttonClose��54     #��[ 56     .��57     �� $��18��58     $��18��59     $'��!/Resources/Images/CloseWindow.png��5:     5;     5<     5=     �� 5?  
   $��1��6   ���#D7B4726   � 5A  
   $��2��6   .+�5B     �� -   5        $��	StarField��5C     
���   �$
}�True��5E     � 5G      5H     �� 5I     $��* �6$   �� 5J     $
��Auto �6$   �� 5K     $
��Auto �6$   �� 5L     $��* �6$    � ColumnDefinitions 5N     �� 5O     $��* �6'   �� 5P     $
��Auto �6'   �� 5Q     $��* �6'   .+�5T     �� $��1��5U     $��1��5V     $��0,0,0,30q�5W     $��Center=�5X     $��40(�5Y     ���#D7B472 	 WelcomeText#��� 	 5[     (� 5]     $��2��5^     $��1��5_     $	��600��5`     $	��400��5a     $��Center=�5b     $	��Top=�5c      (� Stretch$ Uniform=�5d     .��5e     �� $	��450��5f      �� Padding$ 30q�5g     ���	#30000000���#D7B472$��1q�5j     $ 10��5k     ._�5l     �� .+�5m     �� $��0,0,0,2q�5n  !   $��14(�5o  !   ���#D7B472 
 
LoginLabel#��� 
 5q  !   �� 5r     -   5         ��Name$ LoginBox��5r  &   ��68   �� *Email+ �� UpdateSourceTrigger$ PropertyChanged=�" �� ValidatesOnNotifyDataErrors$ False���� 5s     #��[ 5t  !   ��5u  !   �� *
EmailError+y�5v  !   �� *IsEmailErrorVisible+  �� 	Converter#  [ �� 5x     $��0,10,0,2q�5y  !   $��14(�5z  !   ���#D7B472  PasswordLabel#���  5|  !   G� 5}     -   5        ! G�Name$! PasswordBox��5}  *   *  $Microsoft.Xaml.Behaviors.Interaction"   Triggers" 5~  "   +  %Microsoft.Xaml.Behaviors.EventTrigger  5  &   #   	EventName$# PasswordChanged��65   *  $Microsoft.Xaml.Behaviors.TriggerBase$   Actions.$ 5�  *   2  ,Microsoft.Xaml.Behaviors.InvokeCommandAction  %   Command% 6@   �� *PasswordChangedCommand+&   CommandParameter& 6k   �� ' �� ElementName$' PasswordBox��5�  '   5�     �� 5�     #��[ 5�  !   ��5�  !   �� *PasswordError+y�5�  !   �� *IsPasswordErrorVisible+#  [ �� 5�     $��0,25,0,0q�5�  !   $��Center=�5�  !   $ 
Horizontal=�5�  !   .+�5�  "   �� $	��150��5�  %   $��0,0,15,0q�5�  %     LoginButton#���  5�  %   #��[ 5�  %   ��5�  %   �� *LoginCommand+��5�  %   �� $' PasswordBox��5�  %   �� 5�  "   $	��150��5�  %   $��15,0,0,0q�5�  %     RegisterButton#���  5�  %   #��[ 5�  %   ��5�  %   �� *RegisterCommand+5�  %   5�     5�     5�     5�     5�     �� 5�     $��15q�5�     $
��Left=�5�     $��Bottom=�5�     $ Vertical=�5�     .+�5�     �� -	   5        $��	usaButton��5�     $��en-US��5�     #��[ 5�     ��5�     �� *SwitchLanguageCommand+.��5�     �� $��40��5�     $��40��5�     $��/Resources/Images/USA.png��5�     $��Uniform=�5�     5�     �� 5�     -
   5        $��russianButton��5�     $��0,10,0,0q�5�     $��ru-RU��5�     #��[ 5�     ��5�     �� *SwitchLanguageCommand+.��5�     �� $��40��5�     $��40��5�     $"��/Resources/Images/Russia.png��5�     $��Uniform=�5�     5�     5�     5�     5�     5�     