   M S B A M L   `   `   `  ����E  ACinemaMOON, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:CinemaMOON.ViewsCinemaMOON.Views  5      =#clr-namespace:CinemaMOON.ViewModelsCinemaMOON.ViewModels  5	          CinemaMOON.Views.HomePage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5      *localclr-namespace:CinemaMOON.Views  5      Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006  5      ,vm#clr-namespace:CinemaMOON.ViewModels  5	        N� Title$  HomePage��5
         PageBackgroundColor N� 
Background# �   5      *  $Microsoft.Xaml.Behaviors.Interaction   Triggers 5      +  %Microsoft.Xaml.Behaviors.EventTrigger  5   
      	EventName$ Loaded��6   *  $Microsoft.Xaml.Behaviors.TriggerBase   Actions. 5      2  ,Microsoft.Xaml.Behaviors.InvokeCommandAction     Command 6$   �� *LoadCommand+5        5   
   $ Unloaded��6   . 5         6$   �� *UnloadCommand+5       � 	Resources 5        NavButtonStyle �� 
TargetType �� Property	 �� Value
 �� 
TargetType �� CornerRadius �� Triggers P� Property P� Value  LangButtonStyle  FrameLangButtonStyle �� BasedOn  ThemeButtonStyle��% 
  &	       &	 S    &	 �    7  &	 b    �� 5   
   " ��6'   .�5      ��  � ��	 �#D7B4726+   �� 5       � ��$	 70(�6)   �� 5       � ��$
	 Bold"�6+   �� 5       � ��
	 ���� 6+   �� 5       � ��
	 ���� 6,   �� 5        � ��$
	 Auto��6&   �� 5!       � ��$	 40��6)   �� 5"       � ��$
	 Auto��6'   �� 5#       � ��$	 80��6*   �� 5$       �@�Cursor$
	 Hand��6'   �� 5%       �@J�Opacity$		 0.7Q�6(   �� 5&       � ��$
	 10,0q�6(   �� 5'       � ��	 5(      �� 5)      "
 ��6&   .R�5*      �� #��z��6!   $ 5��6K   ._�5+      �� $��Center=�5,   !   $��Center=�5-   !   #��z��5.   !   #��z��5/   !   #��z��50   !   #��z��51   !   52       53      P� 54       �@J�IsMouseOver$
 True��6=   . �55   "   ��  �@J�Opacity$		 1.0Q�6<   �� 56   "    � ��	 �#FFD7006?   57      P� 58       � ��$
 True��6;   . �59   "   ��  � ��	 �#C6A35D6?   5:      P� 5;       � {�$ False��6;   . �5<   "   ��  �@J�Opacity$		 0.4Q�6<   5=      5?      5A      5B      �� 5D   
   " ��6(   .�5E      ��  � ��$	 40��6&   �� 5F       � ��$	 40��6'   �� 5G       � ��
	 ���� 6+   �� 5H       � ��
	 ���� 6,   �� 5I       � ��$	 0q�60   �� 5J       �@�Cursor$
	 Hand��6'   �� 5K       � ��	 5L      �� 5M      "
 ��6&   .R�5N      �� #��z��6!   $ 3��6K   ._�5O      �� $��Center=�6/   $��Center=�6L   5P       5Q      P� 5R       �@J�IsMouseOver$
 True��6=   . �5S   "   ��  � ��	 �	#33FFFFFF6?   5T      P� 5U       � ��$
 True��6;   . �5V   "   ��  � ��	 �	#66FFFFFF6?   5W      5Y      5[      5\      �� 5^   
   8   " ��5a      .�5b      ��  � ��$	 Center=�6;   �� 5c       � ��$	 Center=�69   5d      �� 5f   
   " ��6)   .�5g      ��  � ��$	 40��6&   �� 5h       � ��$	 40��6'   �� 5i       � ��
	 ���� 6+   �� 5j       � ��
	 ���� 6,   �� 5k       � ��$	 0q�60   �� 5l       �@�Cursor$
	 Hand��6'   �� 5m       � ��	 5n      �� 5o      "
 ��6&   .R�5p      �� #��z��6!   $ 3��6K   ._�5q      �� $��Center=�6/   $��Center=�6L   5r       5s      P� 5t       �@J�IsMouseOver$
 True��6=   . �5u   "   ��  � ��	 �	#33FFFFFF6?   5v      P� 5w       � ��$
 True��6;   . �5x   "   ��  � ��	 �	#66FFFFFF6?   5y      5{      5}      5~      5      .��5�      �  � RowDefinitions 5�   
   �� 5�      $
��Auto �6   �� 5�      $��* �6   �� 5�      $
��Auto �6   .+�5�   
   �� $��0��5�      $��	0,20,0,10q�5�      $��Center=�5�      $��Center=�5�      $��30(�5�      $��SemiBold"�5�      ���#D7B472  
FrameTitle#���  5�      � 5�   
   $��1��6    � ColumnDefinitions 5�      �� 5�      $
��Auto �6#   �� 5�      $��* �6#   �� 5�      $
��Auto �6#   .+�5�      �� $��0��5�      $��
15,0,10,15q�5�      $
��Left=�5�      $��Bottom=�5�      .+�5�      �� $��en-US��5�      #��[ 5�      ��5�      �� *SwitchLanguageCommand+.��5�      �� $��40��5�      $��40��5�      $��/Resources/Images/USA.png��5�      $��Uniform=�5�      5�      �� 5�      $��0,10,0,0q�5�      $��ru-RU��5�      #��[ 5�      ��5�      �� *SwitchLanguageCommand+.��5�      �� $��40��5�      $��40��5�      $"��/Resources/Images/Russia.png��5�      $��Uniform=�5�      5�      5�      (� 5�      $��1��5�      $��10q�5�       (� Stretch$ Uniform=�5�      .��5�      � $	��800��6   $	��450��6#    5�      �� 5�      $
��Auto �6+   �� 5�      $��* �6+   �� 5�      $
��Auto �6+   .+�5�      �� $��0��5�      $��10,0,0,0q�5�      $
��Left=�5�      $��Center=�5�       K� ZIndex$	 101��5�      $	��‹��5�      #��[ 5�      ��5�      �� *MovePreviousCommand+5�      �� 5�      -   5         ��Name$ filmCarousel��5�      $��1��5�      $��Center=�5�      $��Center=�5�      ��5�      �� *DisplayItems+��5�      �� 5�      .R�5�   "   �� $��Center=�5�   %   $��Center=�5�   %    K� IsItemsHost$
 True��5�   %    �� Orientation$ 
Horizontal=�5�   %   5�      ��5�      �� 5�       �� DataType1   +CinemaMOON.ViewModels.CarouselItemViewModel"  6+   .R�5�   "   �� $	��200��5�   %   $	��300��5�   %   $��10q�5�   %   
������  �� BitmapScalingMode$ HighQuality=�5�   %    5�   %   �� *ZIndex+ � LayoutTransform 5�   &   �� 5�   *    �� CenterX$	 0.5Q�69    �� CenterY$	 0.5Q�6G    �� ScaleX 6U   �� *ScaleFactor+ �� ScaleY 6t   �� *ScaleFactor+._�5�   &   �� $��UniformToFill=�6H   ��6,   �� *
ImageUri+6H   5�   #   5�      z�5�      T� 5�       T� X$ 0Q�61   5�      �� 5�      $��2��5�      $��0,0,10,0q�5�      $��Right=�5�      $��Center=�5�      $	 101��5�      $	��›��5�      #��[ 5�      ��5�      �� *MoveNextCommand+5�      5�      5�      �� 5�      $��2��5�      $��
10,0,15,15q�5�      $��Right=�5�      $��Bottom=�5�      #��[ 5      ��5�      �� *ToggleThemeCommand+.��5     �� $��40��5     $��40��5     $!��/Resources/Images/Theme.png��5     $��Uniform=�5     5     5     �� 5	  
   $��2��5
     $��3��5     $��15��5     5     5     