   M S B A M L   `   `   `  ����E  ACinemaMOON, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:CinemaMOON.ConvertersCinemaMOON.Converters  5      3clr-namespace:CinemaMOON.ViewsCinemaMOON.Views  5      5clr-namespace:CinemaMOON.ModelsCinemaMOON.Models  5
      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5      =#clr-namespace:CinemaMOON.ViewModelsCinemaMOON.ViewModels  5      (    "CinemaMOON.Views.SeatSelectionPage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      :
converters#clr-namespace:CinemaMOON.Converters     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5      *localclr-namespace:CinemaMOON.Views  5      Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006  5	      ,modelsclr-namespace:CinemaMOON.Models  5
      3system&clr-namespace:System;assembly=mscorlib  5      ,vm#clr-namespace:CinemaMOON.ViewModels  5        N� Title$  Seat Selection��5         PageBackgroundColor N� 
Background# �   5       N� 
Foreground
 ����� � 	Resources 5        GoldAccentBrush �� Color  BrightGoldAccentBrush  PrimaryTextBrush  SecondaryTextBrush  DarkElementBackgroundBrush  SlightlyLighterDarkBrush  ErrorTextBrush  SeatStandardBrush 	 SeatLoveBrush 
 SeatReclinerBrush  SeatSofaBrush  SeatBookedBrush  BookingButtonBrush  BookingButtonHoverBrush  SeatSelectedBrush8   2CinemaMOON.Converters.BooleanToVisibilityConverter   BooleanToVisibilityConverter1   +CinemaMOON.Converters.IsSeatBookedConverter  IsSeatBookedConverter  PageBackButtonStyle �� 
TargetType �� Property �� Value �� 
TargetType	 �� Padding
 �� Triggers P� Property P� Value  SeatBaseStyle  ClickableSeatStyle �� BasedOn �� ScaleX �� ScaleY �� Triggers N� EnterActions �� TargetProperty	 W� To g� Duration R� KeyTime R� Value	 t� To N� ExitActions �� Value �� Binding `� 	Converter �� Path �� RelativeSource �� AncestorType  StandardSeatStyle  LoveSeatStyle  	SofaStyle  ReclinerStyle  AisleSpacerStyle  SmallHallLayoutTemplate �� CornerRadius  HallPage_Screen  �� Orientation
! � Tag  MediumHallLayoutTemplate  LargeHallLayoutTemplate  LegendItemStyle  LegendSeatStyle   LegendTextStyle ! BookingButtonStyle" ��Name# �� 
TargetName��%�F  &	       &	 !     &	 B     &	 c     &	 �     &	 �     &	 �     &	 �     &		     &	
 #    &	 A    &	 a    &	 �    &	 �    &	 �    &	 �    &	 �    &	     7  7  7  &	 �    7  &	 :    7  7  7  7  &	 �    7  &	     7  7 	 &	 �    7  7  &	 i    7  7 
 &	 �    &	      7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  &	 �    7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  &	 
+    7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  &	 �;    &	 /<    7  &	  �=    7  &	! M>    7  7  7  �� 5   
   $ #D7B472��62   �� 5   
   $ #FFD700��68   �� 5   
   $ #E0E0E0��63   �� 5   
   $ #AAAAAA��65   �� 5   
   $ #1C1C1C��6=   �� 5   
   $ #2A2A2A��6;   �� 5   
   $ #FF6B6B��61   �� 5   
   $ Green��64   �� 5   
   $	 Red��60   �� 5   
   $
 Blue��64   �� 5    
   $ Yellow��60   �� 5!   
   $ #555555��62   �� 5"   
   $ #4CAF50��65   �� 5#   
   $ #8BC34A��6:   �� 5$   
   $ #FFD700��64     5&   
   62     5'   
   6+   �� 5)   
   " ��6,   .�5*      ��  � ��
 ���� 6+   �� 5+       � ��8   6+   �� 5,       � ��$ 0q�60   �� 5-       � ��$ 42(�6)   �� 5.       � ��$
 Bold"�6+   �� 5/       � ��$
 10,0q�6(   �� 50       � ��$ 0q�6'   �� 51       � ��$ Center=�62   �� 52       � ��$
 Left=�64   �� 53       �@�Cursor$
 Hand��6'   �� 54       � �� 55      �� 56      " ��6&   .R�57      �� #	 z��6!   #��z��6E   ._�58      �� $��Center=�59   !   $��Center=�5:   !   #��z��5;   !   #��z��5<   !   #��z��5=   !   $	��←��5>   !   5?      
 5@      P� 5A       �@J�IsMouseOver$
 True��6=   . �5B   "   �� 8   � ��6?   5C      P� 5D       � {�$ False��6;   . �5E   "   ��  �@J�Opacity$	 0.5Q�6<   �� 5F   "    � �� �#8886?   5G      P� 5H       � ��$
 True��6;   . �5I   "   �� 8   � ��6?   5J      5L      5N      5O      �� 5Q   
   " ��6&   .�5R      ��  � ��$ 26��6&   �� 5S       � ��$ 26��6'   �� 5T       � ��$ 3q�6'   �� 5U       �@��CornerRadius$ 3��6-   �� 5V       � ��$ Center=�62   �� 5W       � ��$ 1q�60   �� 5X       � �� �#4446,   �� 5Y       � ��8   6+   5Z      �� 5\   
   8   " ��5_      .�5`      ��  �@�Cursor$
 Hand��6'   �� 5a       �@J�RenderTransformOrigin$ 0.5,0.5&�66   �� 5b       � z� 5c      �� 5d      $	 1.0Q�6%   $	 1.0Q�62   5f       5g      P� 5h       �@J�IsMouseOver$
 True��61    5i      �� 5j      .��5k      �� .��5l   "   W� $ RenderTransform.ScaleX�5m   %    1.155n   %   $ 0:0:0.1D�5o   %   W� 5p   "   $ RenderTransform.ScaleY�5q   %    1.155r   %   $ 0:0:0.1D�5s   %   T� 5t   "   $ BorderBrush�6@   ..�5u   &   i� $ 0��6=   8  6I   5v   #   t� 5w   "   $ BorderThickness�5x   %    1.55y   %   $ 0:0:0.1D�5z   %   5{      5|       5~      �� 5      .��5�      �� .��5�   "   W� $ RenderTransform.ScaleX�5�   %    1.05�   %   $ 0:0:0.1D�5�   %   W� 5�   "   $ RenderTransform.ScaleY�5�   %    1.05�   %   $ 0:0:0.1D�5�   %   T� 5�   "   $ BorderBrush�6@   ..�5�   &   i� $ 0��6=    5�   *   �� 5�   .   $
 #444��6>   5�   '   5�   #   t� 5�   "   $ BorderThickness�5�   %    1.05�   %   $ 0:0:0.1D�5�   %   5�      5�      5�      �� 5�       True6    5�      `� 5�      8  .1�5�      �� $	 Tag�6&    61   �� *Self+�� 5�      $ DataContext.BookedSeatIds�6&    6G   ��  Page5�      .a�5�      ��  � ��8  63   �� 5�       � {�$ False��62   �� 5�       �@�Cursor$ No��6/   �� 5�       � z� 5�      �� 5�      $	 1.0Q�6-   $	 1.0Q�6:   5�      �� 5�       � ��$ 1q�68   5�      5�      �� 5�   
   8   " ��5�      .�5�      ��  �@^�ToolTip 67   �� *ToolTipText+5�      �� 5�   
   8   " ��5�      .�5�      ��  � ��$ 58��6&   �� 5�       � ��8  6+   �� 5�       �@^�ToolTip 67   �� *ToolTipText+5�      �� 5�   
   8   " ��5�      .�5�      ��  � ��$ 58��6&   �� 5�       � ��8  6+   �� 5�       �@^�ToolTip 67   �� *ToolTipText+5�      �� 5�   
   8   " ��5�      .�5�      ��  � ��8  6+   �� 5�       �@^�ToolTip 67   �� *ToolTipText+5�      �� 5�   
   " ��6)   .�5�      ��  � ��$ 26��6&   �� 5�       � ��$ 26��6'   �� 5�       � ��$ 4q�6'   �� 5�       � ��
 ���� 6+   �� 5�       � ��$ Center=�62   �� 5�       � ��$ 0q�60   �� 5�       �@�Cursor$ Arrow��6'   5�      �� 5�   
   .R�5�      �� $��Center=�6   $	��Top=�66   .+�5�      �� $	��400��5�      $��0,0,0,30q�5�      $
	 10,5q�5�      ���#333���#D7B472$��0,0,0,2q�5�      $ 	10,10,0,0��5�      ._�5�      �� $��Center=�5�      $��18(�5�      $
��Bold"�5�      ���#D7B472#���  5�      5�      �� 5�      $	��0,5q�5�      $��Center=�5�      $  
Horizontal=�5�      .+�5�      �� -   5        8��  $
! R1S1��5�      �� 5�      -   5        8�� $
! R1S2��5�      �� 5�      -   5        8�� $
! R1S3��5�      �� 5�      -   5        8�� $
! R1S4��5�      �� 5�      8�� 6   �� 5�      -   5        8�� $
! R1S5��5      �� 5     -   5        8�� $
! R1S6��5     �� 5     -   5        8�� $
! R1S7��5     �� 5	     -   5        8�� $
! R1S8��5     �� 5     8��	 6   �� 5     -	   5        8��
 $
! R1S9��5     �� 5     -
   5        8�� $! R1S10��5     �� 5     -   5        8�� $! R1S11��5     �� 5     -   5        8�� $! R1S12��5     5     �� 5     $	��0,5q�5      $��Center=�5!     $  
Horizontal=�5"     .+�5#     �� -   5        8�� $
! R2S1��5&     �� 5'     -   5        8�� $
! R2S2��5*     �� 5+     -   5        8�� $
! R2S3��5.     �� 5/     -   5        8�� $
! R2S4��52     �� 53     8�� 6   �� 54     -   5        8�� $
! R2S5��57     �� 58     -   5        8�� $
! R2S6��5;     �� 5<     -   5        8�� $
! R2S7��5?     �� 5@     -   5        8�� $
! R2S8��5C     �� 5D     8�� 6   �� 5E     -   5        8�� $
! R2S9��5H     �� 5I     -   5        8�� $! R2S10��5L     �� 5M     -   5        8�� $! R2S11��5P     �� 5Q     -   5        8�� $! R2S12��5T     5U     �� 5V     $	��0,5q�5W     $��Center=�5X     $  
Horizontal=�5Y     .+�5Z     �� -   5        8�� $
! R3S1��5]     �� 5^     -   5        8�� $
! R3S3��5a     �� 5b     8�� 6   �� 5c     -   5        8�� $
! R3S5��5f     �� 5g     -   5        8��  $
! R3S7��5j     �� 5k     8��! 6   �� 5l     -   5        8��" $
! R3S9��5o     �� 5p     -   5        8��# $! R3S11��5s     5t     �� 5u     $	��0,5q�5v     $��Center=�5w     $  
Horizontal=�5x     .+�5y     �� -   5        8��$ $
! R4S1��5|     �� 5}     -    5        8��% $
! R4S2��5�     �� 5�     -!   5        8��& $
! R4S3��5�     �� 5�     $��58��6   8��' 6(   �� 5�     -"   5        8��( $
! R4S4��5�     �� 5�     -#   5        8��) $
! R4S5��5�     �� 5�     -$   5        8��* $
! R4S6��5�     �� 5�     $��58��6   8��+ 6(   �� 5�     -%   5        8��, $
! R4S7��5�     �� 5�     -&   5        8��- $
! R4S8��5�     �� 5�     -'   5        8��. $
! R4S9��5�     5�     5�     5�     �� 5�  
   .R�5�     �� $��Center=�6   $	��Top=�66   .+�5�     �� $	��450��5�     $��0,0,0,30q�5�     $
	 10,5q�5�     ���#333���#D7B472$��0,0,0,2q�5�     $ 	10,10,0,0��5�     ._�5�     �� $��Center=�5�     $��18(�5�     $
��Bold"�5�     ���#D7B472#���  5�     5�     �� 5�     $	��0,5q�5�     $��Center=�5�     $  
Horizontal=�5�     .+�5�     �� -(   5        8��  $
! R1S1��5�     �� 5�     -)   5        8�� $
! R1S3��5�     �� 5�     8�� 6   �� 5�     -*   5        8�� $
! R1S5��5�     �� 5�     -+   5        8�� $
! R1S7��5�     �� 5�     8�� 6   �� 5�     -,   5        8�� $
! R1S9��5�     �� 5�     --   5        8�� $! R1S11��5�     5�     �� 5�     $	��0,5q�5�     $��Center=�5�     $  
Horizontal=�5�     .+�5�     �� -.   5        8�� $
! R2S1��5�     �� 5�     -/   5        8��	 $
! R2S2��5�     �� 5�     -0   5        8��
 $
! R2S3��5�     �� 5�     -1   5        8�� $
! R2S4��5�     �� 5�     8�� 6   �� 5�     -2   5        8�� $
! R2S5��5�     �� 5�     -3   5        8�� $
! R2S6��5�     �� 5�     -4   5        8�� $
! R2S7��5�     �� 5�     -5   5        8�� $
! R2S8��5�     �� 5�     8�� 6   �� 5�     -6   5        8�� $
! R2S9��5�     �� 5�     -7   5        8�� $! R2S10��5      �� 5     -8   5        8�� $! R2S11��5     �� 5     -9   5        8�� $! R2S12��5     5	     �� 5
     $	��0,5q�5     $��Center=�5     $  
Horizontal=�5     .+�5     �� -:   5        8�� $
! R3S1��5     �� 5     -;   5        8�� $
! R3S2��5     �� 5     -<   5        8�� $
! R3S3��5     �� 5     -=   5        8�� $
! R3S4��5     �� 5     8�� 6   �� 5     ->   5        8�� $
! R3S5��5"     �� 5#     -?   5        8�� $
! R3S6��5&     �� 5'     -@   5        8�� $
! R3S7��5*     �� 5+     -A   5        8�� $
! R3S8��5.     �� 5/     8�� 6   �� 50     -B   5        8��  $
! R3S9��53     �� 54     -C   5        8��! $! R3S10��57     �� 58     -D   5        8��" $! R3S11��5;     �� 5<     -E   5        8��# $! R3S12��5?     5@     �� 5A     $	��0,5q�5B     $��Center=�5C     $  
Horizontal=�5D     .+�5E     �� -F   5        8��$ $
! R4S1��5H     �� 5I     -G   5        8��% $
! R4S2��5L     �� 5M     -H   5        8��& $
! R4S3��5P     �� 5Q     -I   5        8��' $
! R4S4��5T     �� 5U     8��( 6   �� 5V     -J   5        8��) $
! R4S5��5Y     �� 5Z     -K   5        8��* $
! R4S6��5]     �� 5^     -L   5        8��+ $
! R4S7��5a     �� 5b     -M   5        8��, $
! R4S8��5e     �� 5f     8��- 6   �� 5g     -N   5        8��. $
! R4S9��5j     �� 5k     -O   5        8��/ $! R4S10��5n     �� 5o     -P   5        8��0 $! R4S11��5r     �� 5s     -Q   5        8��1 $! R4S12��5v     5w     �� 5x     $	��0,5q�5y     $��Center=�5z     $  
Horizontal=�5{     .+�5|     �� -R   5        8��2 $
! R5S1��5     �� 5�     -S   5        8��3 $
! R5S3��5�     �� 5�     8��4 6   �� 5�     -T   5        8��5 $
! R5S5��5�     �� 5�     -U   5        8��6 $
! R5S7��5�     �� 5�     8��7 6   �� 5�     -V   5        8��8 $
! R5S9��5�     �� 5�     -W   5        8��9 $! R5S11��5�     5�     5�     5�     �� 5�  
   .R�5�     �� $��Center=�6   $	��Top=�66   .+�5�     �� $	��450��5�     $��0,0,0,30q�5�     $
	 10,5q�5�     ���#333���#D7B472$��0,0,0,2q�5�     $ 	10,10,0,0��5�     ._�5�     �� $��Center=�5�     $��18(�5�     $
��Bold"�5�     ���#D7B472#���  5�     5�     �� 5�     $	��0,5q�5�     $��Center=�5�     $  
Horizontal=�5�     .+�5�     �� -X   5        8��  $
! R1S1��5�     �� 5�     -Y   5        8�� $
! R1S3��5�     �� 5�     8�� 6   �� 5�     -Z   5        8�� $
! R1S5��5�     �� 5�     -[   5        8�� $
! R1S7��5�     �� 5�     8�� 6   �� 5�     -\   5        8�� $
! R1S9��5�     �� 5�     -]   5        8�� $! R1S11��5�     5�     �� 5�     $	��0,5q�5�     $��Center=�5�     $  
Horizontal=�5�     .+�5�     �� -^   5        8�� $
! R2S1��5�     �� 5�     -_   5        8��	 $
! R2S2��5�     �� 5�     -`   5        8��
 $
! R2S3��5�     �� 5�     -a   5        8�� $
! R2S4��5�     �� 5�     8�� 6   �� 5�     -b   5        8�� $
! R2S5��5�     �� 5�     -c   5        8�� $
! R2S6��5�     �� 5�     -d   5        8�� $
! R2S7��5�     �� 5�     -e   5        8�� $
! R2S8��5�     �� 5�     8�� 6   �� 5�     -f   5        8�� $
! R2S9��5�     �� 5�     -g   5        8�� $! R2S10��5�     �� 5�     -h   5        8�� $! R2S11��5�     �� 5�     -i   5        8�� $! R2S12��5�     5      �� 5     $	��0,5q�5     $��Center=�5     $  
Horizontal=�5     .+�5     �� -j   5        8�� $
! R3S1��5     �� 5	     -k   5        8�� $
! R3S2��5     �� 5     -l   5        8�� $
! R3S3��5     �� 5     -m   5        8�� $
! R3S4��5     �� 5     8�� 6   �� 5     -n   5        8�� $
! R3S5��5     �� 5     -o   5        8�� $
! R3S6��5     �� 5     -p   5        8�� $
! R3S7��5!     �� 5"     -q   5        8�� $
! R3S8��5%     �� 5&     8�� 6   �� 5'     -r   5        8��  $
! R3S9��5*     �� 5+     -s   5        8��! $! R3S10��5.     �� 5/     -t   5        8��" $! R3S11��52     �� 53     -u   5        8��# $! R3S12��56     57     �� 58     $	��0,5q�59     $��Center=�5:     $  
Horizontal=�5;     .+�5<     �� -v   5        8��$ $
! R4S1��5?     �� 5@     -w   5        8��% $
! R4S2��5C     �� 5D     -x   5        8��& $
! R4S3��5G     �� 5H     -y   5        8��' $
! R4S4��5K     �� 5L     8��( 6   �� 5M     -z   5        8��) $
! R4S5��5P     �� 5Q     -{   5        8��* $
! R4S6��5T     �� 5U     -|   5        8��+ $
! R4S7��5X     �� 5Y     -}   5        8��, $
! R4S8��5\     �� 5]     8��- 6   �� 5^     -~   5        8��. $
! R4S9��5a     �� 5b     -   5        8��/ $! R4S10��5e     �� 5f     -�   5        8��0 $! R4S11��5i     �� 5j     -�   5        8��1 $! R4S12��5m     5n     �� 5o     $	��0,5q�5p     $��Center=�5q     $  
Horizontal=�5r     .+�5s     �� -�   5        8��2 $
! R5S1��5v     �� 5w     -�   5        8��3 $
! R5S3��5z     �� 5{     8��4 6   �� 5|     -�   5        8��5 $
! R5S5��5     �� 5�     -�   5        8��6 $
! R5S7��5�     �� 5�     8��7 6   �� 5�     -�   5        8��8 $
! R5S9��5�     �� 5�     -�   5        8��9 $! R5S11��5�     5�     �� 5�     $	��0,5q�5�     $��Center=�5�     $  
Horizontal=�5�     .+�5�     �� -�   5        8��: $
! R6S1��5�     �� 5�     -�   5        8��; $
! R6S2��5�     �� 5�     -�   5        8��< $
! R6S3��5�     �� 5�     $��58��6   8��= 6(   �� 5�     -�   5        8��> $
! R6S4��5�     �� 5�     -�   5        8��? $
! R6S5��5�     �� 5�     -�   5        8��@ $
! R6S6��5�     �� 5�     $��58��6   8��A 6(   �� 5�     -�   5        8��B $
! R6S7��5�     �� 5�     -�   5        8��C $
! R6S8��5�     �� 5�     -�   5        8��D $
! R6S9��5�     5�     5�     5�     �� 5�  
   " ��6(   .�5�     ��  �@��Orientation$ 
Horizontal=�6,   �� 5�      � ��$	 0,5q�6'   5�     �� 5�  
   8   " ��5�     .�5�     ��  �@�Cursor$ Arrow��6'   �� 5�      � ��$ 0,0,10,0q�6'   �� 5�      � ��$ 22��6&   �� 5�      � ��$ 22��6'    5�     P� 5�      �@�Tag$
 Love��6)   . �5�     ��  � ��$ 48��6.   5�     P� 5�      �@�Tag$
 Sofa��6)   . �5�     ��  � ��$ 48��6.   5�     5�     �� 5�  
   " ��6(   .�5�     ��  � ��8   6+   �� 5�      � ��$ 16(�6)   �� 5�      � ��$ Center=�62   5�     �� 5�  
   " ��6+   .�5�     ��  � ��8   6+   �� 5�      � ��
 �����6+   �� 5�      � ��$ 0q�60   �� 5�      � ��$ 25,12q�6(   �� 5�      � ��$ 20(�6)   �� 5�      � ��$ SemiBold"�6+   �� 5�      �@�Cursor$
 Hand��6'   �� 5�      � ��$ Center=�64   �� 5�      � �� 5�     �� 5�     " ��6&   .R�5�     �� $" border��5�     #��z��5�     $ 5��5�     ._�5�     �� #��z��5�  !   $��Center=�5�  !   $��Center=�5�  !   5�     
 5�     P� 5�      �@J�IsMouseOver$
 True��6=   . �5�  "   �� $# border��6)   8   � ��6S   5�     P� 5�      � ��$
 True��6;   . �5�  "   �� $# border��6)   8   � ��6S   �� 5�  "    �@J�Opacity$	 0.8Q�6<   5�     P� 5�      � {�$ False��6;   . �5�  "   ��  �@J�Opacity$	 0.5Q�6<   �� 5�  "   $# border��6)    � �� �#8886S   �� 5�  "    �@�Cursor$ No��6;   5�     5�     5�     5�     5�     .��5      � $��10q�6   $ � RowDefinitions$ 5  
   �� 5     $
��Auto �6   �� 5     $
��Auto �6   �� 5     $��* �6   �� 5     $
��Auto �6   .+�5  
   � $��0��6   $��5,5,5,0q�6   % � ColumnDefinitions% 5     �� 5	     $
��Auto �6#   �� 5
     $��* �6#   .+�5     �� $��0��5     #��[ 5     ��5     �� *GoBackCommand+��5     ��  ��  Page5     �� 5     $��1��5     $��Center=�5     $��Center=�5     $��30(�5     $��SemiBold"�5     #��[ 5      " SeatSelectionPage_Title#��� " 5     5     �� 5  
   $��1��5     $��	5,10,5,10q�5     $
	 10,5q�5     #��[ 5     $ 3��5     ._�5      �� $��Center=�6   $  
Horizontal=�66   .+�5!     �� $��0,0,5,0q�5"     $��18(�5#     $��SemiBold"�5$     #��[ 5%     ��5&     �� *
MovieTitle+& �� Mode$& OneWay=�' �� FallbackValue$' 
Фильм���� 5'     $��0,0,5,0q�5(     $��18(�5)     #��[ 5*     $	�� | ��5+     �� 5,     $��0,0,5,0q�5-     $��18(�5.     #��[ 5/     ��50     �� *
HallName+$& OneWay=�$' Зал���� 51     $��0,0,5,0q�52     $��18(�53     #��[ 54     $	�� | ��55     �� 56     $��18(�57     #��[ 58     ��59     �� *
ShowTime+$& OneWay=�( �� StringFormat$( dd MMMM yyyy, HH:mm��$' Дата и время��5:     5;     � 5<  
   $��2��6   $��5,0,5,0q�6   % 5=     �� 5>     $��* �6#   �� 5?     $
��Auto �6#   .+�5A     �� $��0��5B     $
��Auto=�5C     $
��Auto=�5D     .��5E     �� $��Center=�5F     $��Center=�5G     ��5H     �� ��5I     �� 5J     " ��6    .�5K     ��  � �� 6@   Z�  5L     �� 5M  "    6.   �� *CurrentHallType+	 small6R   .a�5N  &   ��  � ��# [ 6H   5O  #   �� 5P  "    6.   �� *CurrentHallType+
 medium6R   .a�5Q  &   ��  � ��# [ 6H   5R  #   �� 5S  "    6.   �� *CurrentHallType+	 large6R   .a�5T  &   ��  � ��# [ 6H   5U  #   5W     5Y     5Z     �� 5[     $��1��5\     $	��250��5]     $��15,0,0,0q�5^     $	 15q�5_     $��Right=�5`     $	��Top=�5a     #��[ 5b     $ 5��5c     ._�5d     �� .+�5e     �� $��0,0,0,15q�5f     $��22(�5g     $��SemiBold"�5h     #��[ 5i      # Legend_Title#��� # 5j     �� 5k     #��[ 6!   .+�5l     �� #��[ 6!   #��[ 6Q   �� 5m     #��[  6$   �� 6N   $��18(�6R    .$ *SeatSelectionPage_Legend_SeatType_Standard#�� $ 6`    �� 6�   $��SemiBold"�6�   $�	 - 15 BYN��6�   6�   5n     �� 5o     #��[ 6!   .+�5p     �� #��[	 5q     #��[ 5r     $
! Love��5s     �� 5t     #��[  6$   �� 6N   $��18(�6R    % HallPage_SeatType_LoveSeat#�� % 6`    �� 6�   $��SemiBold"�6�   $�	 - 30 BYN��6�   6�   5u     �� 5v     #��[ 6!   .+�5w     �� #��[ 5x     #��[ 5y     $
! Sofa��5z     �� 5{     #��[  6$   �� 6N   $��18(�6R    *& &SeatSelectionPage_Legend_SeatType_Sofa#�� & 6`    �� 6�   $��SemiBold"�6�   $�	 - 35 BYN��6�   6�   5|     �� 5}     #��[ 6!   .+�5~     �� #��[
 6!   #��[ 6Q   �� 5     #��[  6$   �� 6N   $��18(�6R    .' *SeatSelectionPage_Legend_SeatType_Recliner#�� ' 6`    �� 6�   $��SemiBold"�6�   $�	 - 20 BYN��6�   6�   5�     �� 5�     $
��0,10q�6    #��[ 6.   �� 5�     #��[ 6!   .+�5�     �� #��[ 6!   #��[ 6O   �� 5�     #��[  6$    ( Legend_Booked#��� ( 6M   5�     5�     5�     5�     �� 5�  
   $��3��5�     $��5,10,5,5q�5�     $	 15,10q�5�     #��[ 5�     $ 3��5�     ._�5�     � % 5�     �� 5�     $��* �6'   �� 5�     $
��Auto �6'   .+�5�     �� $��0��5�     $��Center=�5�     $��18(�5�     #��[ 5�     $
��Wrap=�5�     �� 6*    () $SeatSelectionPage_SelectedSeatsLabel#�� ) 6.   �� 6o   $� ��6s   �� 6   $��SemiBold"�6�   �6�   �� *SelectedSeatsText+$& OneWay=�$	' ---��6�   �� 5�     $��1��5�     $��15,0,0,0q�5�      #* SeatSelectionPage_ConfirmButton#��� * 5�     #��[! 5�     ��5�     �� *ConfirmBookingCommand+{�5�     �� *CanConfirmBooking+5�     5�     5�     5�     5�     