   M S B A M L   `   `   `  ����E  ACinemaMOON, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:CinemaMOON.ConvertersCinemaMOON.Converters  5      3clr-namespace:CinemaMOON.ViewsCinemaMOON.Views  5      =#clr-namespace:CinemaMOON.ViewModelsCinemaMOON.ViewModels  5	      %    CinemaMOON.Views.AddingFilmPage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      :
converters#clr-namespace:CinemaMOON.Converters     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:CinemaMOON.Views     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      2vm#clr-namespace:CinemaMOON.ViewModels     5	        N� Title$  AddingFilmPage��5
         PageBackgroundColor N� 
Background# �   5       � 	Resources 5        GoldAccentBrush �� Color  BrightGoldAccentBrush  PrimaryTextBrush  DarkElementBackgroundBrush  SlightlyLighterDarkBrush  SelectionBackgroundBrush  GridLineBrush  ErrorTextBrush8   2CinemaMOON.Converters.BooleanToVisibilityConverter  	 BooleanToVisibilityConverter 
 AdminButtonStyle �� 
TargetType �� Property �� Value �� 
TargetType ��Name	 �� CornerRadius
 �� Triggers P� Property P� Value �� 
TargetName  AdminTextBoxStyle ��Name$ �� HorizontalScrollBarVisibility" �� VerticalScrollBarVisibility �� Triggers �� RelativeSource �� Path   AdminDescriptionTextBoxStyle �� BasedOn " AdminComboBoxToggleButtonStyle � ColumnDefinitions F�Name  AdminComboBoxStyle �Name J� SnapsToDevicePixels �Name � AllowsTransparency �� MaxDropDownHeight �� ElementName ��Name-  'System.Windows.Input.KeyboardNavigation   DirectionalNavigation d�Name  d� 	IsChecked! �� Mode" ��Name# �� SelectionBoxItem#$ �� SelectionBoxItemStringFormat% �� ContentStringFormat& �� SelectionBoxItemTemplate' J� IsHitTestVisible( �� Padding  AdminLabelStyle  PreviewImageStyle  AdminScrollThumbStyle  AdminScrollBarStyle) ]�Name* J� IsMouseOver+ ]� DecreaseRepeatButton, �� LineUpCommand- ]� IncreaseRepeatButton. �� LineDownCommand/ ]� Thumb0 �� LineLeftCommand1 �� LineRightCommand  AdminScrollViewerStyle2 ��Name3 �� CanContentScroll4 �� CanHorizontallyScroll5 �� CanVerticallyScroll6 ��Name4  .System.Windows.Automation.AutomationProperties7   AutomationId8 � Cursor9 �� ScrollableHeight: � Maximum; � Minimum< �� ViewportHeight= �� ViewportSize*> �� #ComputedVerticalScrollBarVisibility? � Value@ �� ScrollableWidthA �� OrientationB �� ViewportWidth,C �� %ComputedHorizontalScrollBarVisibility  PageBackButtonStyle  ErrorTextStyle��%�2  &	       &	 !     &	 B     &	 c     &	 �     &	 �     &	 �     &	 �     &		     &	
     7  7  7  7  7  7  7  7  &	 �    7  7  7  7  7  7  7  &	 �    7  &	 v    7  7  7  7  7  &	 B    7  7  7  7  7  7  7  7  7  '�� �    7  7  7  7  7  7  7  7  7  &	 �    7  &	 ]    &	      7  7  7  &	 W"    7  7  7  7  7  &	 �'    7  7  &	 �,    7  7  7  &	 �0    7  �� 5   
   $ #D7B472��62   �� 5   
   $ #FFD700��68   �� 5   
   $ #E0E0E0��63   �� 5   
   $ #1C1C1C��6=   �� 5   
   $ #2A2A2A��6;   �� 5   
   $ #4A4A4A��6;   �� 5   
   $
 #444��60   �� 5   
   $ #FF6B6B��61     5   
   62   �� 5   
   " ��6)   .�5      ��  � ��8   6+   �� 5       � ��8  6+   �� 5        � ��$ 1q�60   �� 5!       � ��8  6,   �� 5"       � ��$ 15,10q�6(   �� 5#       � ��$ 5q�6'   �� 5$       � ��$	 120��6)   �� 5%       � ��$ 20(�6)   �� 5&       �@�Cursor$
 Hand��6'   �� 5'       � ��$ Center=�69   �� 5(       � �� 5)      �� 5*      " ��6&   .R�5+      �� $ border��5,      #��z��5-      #��z��5.      #��z��5/      $	 3��50      ._�51      �� $��Center=�6/   $��Center=�6L   52      
 53      P� 54       �@J�IsMouseOver$
 True��6=   . �55   "   �� $ border��6)   8   � ��6S   �� 56   "   8   � ��6@   �� 57   "   8   � ��6?   58      P� 59       � ��$
 True��6;   . �5:   "   �� $ border��6)    � �� �#3535356S   �� 5;   "   8   � ��6@   �� 5<   "   8   � ��6?   5=      P� 5>       � {�$ False��6;   . �5?   "   ��  �@J�Opacity$	 0.5Q�6<   �� 5@   "   $ border��6)    � �� �#3838386S   �� 5A   "    � �� �#5556@   �� 5B   "    � �� �#9996?   5C      5E      5G      5H      �� 5J   
   " ��6*   .�5K      ��  � ��8   6+   �� 5L       � ��8  6+   �� 5M       �@��
CaretBrush8  6+   �� 5N       � ��8  6,   �� 5O       � ��$ 1q�60   �� 5P       � ��$	 8,6q�6(   �� 5Q       � ��$ 
10,0,10,10q�6'   �� 5R       � ��$ 18(�6)   �� 5S       � ��$ Center=�69   �� 5T       � �� 5U      �� 5V      " ��6&   .R�5W      �� $ Bd��5X      #��z��5Y      #��z��5Z      #��z��5[      $	 3��5\      ._�5]      �� $ PART_ContentHost��5^   !   $|�false��5_   !   #��z 5`   !   #��z 5a   !   5b      
 5c      P� 5d       �@J�IsMouseOver$
 True��6=   . �5e   "   �� $ Bd��6)   8   � ��6P   5f      P� 5g       �@J�	IsFocused$
 True��6;   . �5h   "   �� $ Bd��6)   8   � ��6P   �� 5i   "   $ Bd��6)    � ��$ 1q�6T   5j      P� 5k       � {�$ False��6;   . �5l   "   ��  �@J�Opacity$	 0.6Q�6<   �� 5m   "   $ Bd��6)    � �� �#2828286O   �� 5n   "   $ Bd��6)    � �� �#4446P   �� 5o   "    � �� �#8886?   5p      5r      5t      �� 5u       �@=�ErrorTemplate 69   Z� �� 5v       � ��$ Hidden=�6K   �� 5w       � ��$ Hidden=�6I    5x      P� 5y       �@=�HasError$
 True��69   . �5z      ��  � ��8  64   �� 5{       �@�ToolTip 60   ��  �� *Self+$) #(Validation.Errors)[0].ErrorContent�5|      5~      �� 5�   
   8   " ��5�      .�5�      ��  �@��AcceptsReturn$
 True��6.   �� 5�       �@��TextWrapping$
 Wrap=�6-   �� 5�       � ��$	 Top=�69   �� 5�       � ��$
 Auto=�6I   �� 5�       � ��$
 Auto=�6K   �� 5�       � ��$ 	10,0,10,0q�6'   �� 5�       � ��$	 180��6*   �� 5�       � ��$ 18(�6)   �� 5�       � ��$	 8,6q�6(   5�      �� 5�   
   " d�67   .�5�      ��  � ��8   6+   �� 5�       � ��8  6,   �� 5�       � ��$ 5q�6(   �� 5�       � �� 5�      �� 5�      " d�6&   .R�5�      �� $��ToggleButtonBorder��5�      #��z��5�      #��z��5�      $��0q�5�      ._�5�      �  5�   "   �� 5�   &   $��* �67   �� 5�   &   $
��Auto �67   .+�5�   "   F� $ Arrow��5�   %   $��1��5�   %   $��0,0,8,0q�5�   %   $��Right=�5�   %   $��Center=�5�   %   "����        � 	=  	= � z     	8�� 5�   %   5�      5�      
 5�      P� 5�       �@J�IsMouseOver$
 True��6=   . �5�   "   �� $ Arrow��6)   8   � ��6L   5�      P� 5�       �@d�	IsChecked$
 TrueY�6;   . �5�   "   �� $ Arrow��6)   8   � ��6L   5�      P� 5�       � {�$ False��6;   . �5�   "   �� $ Arrow��6)    � �� �#6666L   �� 5�   "    �@J�Opacity$	 0.6Q�6<   5�      5�      5�      5�      �� 5�   
   " ��6+   .�5�      ��  � ��8   6+   �� 5�       � ��8  6+   �� 5�       � ��8  6,   �� 5�       � ��$ 1q�60   �� 5�       � ��$	 8,6q�6(   �� 5�       � ��$ 
10,0,10,10q�6'   �� 5�       � ��$ 18(�6)   �� 5�       � ��$ Center=�69   �� 5�       � ��$ Disabled=�6K   �� 5�       � ��$
 Auto=�6I   �� 5�       � �� 5�      �� 5�      " ��6&   .R�5�      � $ templateRoot��6   $
 true��65    5�      �� 5�   "   $��* �63   �� 5�   "   $
��Auto �63   .+�5�      � $ 
PART_Popup��5�   !   $��2��5�   !   $
 true��5�   !   $��Bottom=�5�   !   #��� .�5�   !   ��5�   !   �� *IsDropDownOpen+ �� *TemplatedParent+.��5�   "   �� $ dropDownBorder��5�   %   #��z 5�   %   8�� 8�� $��1q�5�   %   ��5�   %   �� *ActualWidth+$ templateRoot��._�5�   &   �� $ DropDownScrollViewer��63   .��5�   *   �� $ ItemsPresenter��5�   -   $ 	Contained=�5�   -   # z 5�   -   5�   '   5�   #   5�      �� 5�      $ 
MainBorder��5�   !   $��2��5�   !   #��z��5�   !   #��z��5�   !   #��z��5�   !   $	 3��5�   !   d� 5�      $ toggleButton��5�   !   $��0��5�   !   $��2��5�   !   
������ 
������ 8��   5�   !   �� *IsDropDownOpen+$! TwoWay=� �� *TemplatedParent+5�   !   �� 5�      $" contentPresenter��5�   !   $��0��5�   !   #��z��5�   !   #��z��5�   !   #��z��5�   !   #��z# 5�   !   #% z$ 5�   !   #��z& 5�   !   $' false��5�   !   # z 5�   !   5�      
 5�      P� 5       �@J�IsMouseOver$
 True��6=   . �5  "   �� $ 
MainBorder��6)   8   � ��6X   5     P� 5      �@J�	IsFocused$
 True��6;   . �5  "   �� $ 
MainBorder��6)   8   � ��6X   5     P� 5      � {�$ False��6;   . �5  "   ��  �@J�Opacity$	 0.6Q�6<   �� 5  "   $ 
MainBorder��6)    � �� �#2828286W   �� 5	  "   $ 
MainBorder��6)    � �� �#4446X   �� 5
  "    � �� �#8886?   5     5     5     �� 5      �@=�ErrorTemplate 69   Z�  5     P� 5      �@=�HasError$
 True��69   . �5     ��  � ��8  64   �� 5      �@�ToolTip 60   ��  �� *Self+$) #(Validation.Errors)[0].ErrorContent�5     5     �� 5  
   " ��6   .�5     ��  � ��8   6+   �� 5      � ��8  6+   �� 5      � ��$	 8,6q�6(   �� 5      � ��$ 16(�6)   �� 5      � ��$ 0,0,0,1q�60   �� 5      � ��8  6,   �� 5       � �� 5!     �� 5"     " ��6&   .R�5#     �� $��Bd��5$     #( z��5%     #��z��5&     #��z��5'     #��z��5(     $
 true��5)     ._�5*     �� #��z��5+  !   #��z��5,  !   # z 5-  !   5.     
 5/     P� 50      �@��IsHighlighted$
 True��6?   . �51  "   �� $ Bd��6)   8   � ��6O   �� 52  "   8   � ��6?   �� 53  "   $ Bd��6)   8   � ��6P   54     P� 55      �@��
IsSelected$
 True��6<   . �56  "   �� $ Bd��6)   8   � ��6O   �� 57  "   8   � ��6?   �� 58  "   $ Bd��6)   8   � ��6P   59     P� 5:      � {�$ False��6;   . �5;  "   ��  � �� �#8886?   �� 5<  "   $ Bd��6)    � �� �#5556P   5=     5?     5A     5B     �� 5D  
   " ��6(   .�5E     ��  � ��8   6+   �� 5F      � ��$ Center=�62   �� 5G      � ��$ 
10,10,10,5q�6'   �� 5H      � ��$ 20(�6)   5I     �� 5K  
   " ��6*   .�5L     ��  � ��$ 5q�6'   �� 5M      � ��$ Uniform=�6(   �� 5N      � ��$ Center=�64   �� 5O      � ��$ Center=�62   5P     �� 5R  
   " n�6.   .�5S     ��  �@�OverridesDefaultStyle$
 true��66   �� 5T      � ��$ false��6*   �� 5U      � �� 5V     �� 5W     " n�6&   .R�5X     �� $ 	rectangle��5Y     #��z��5Z     #��z��5[     8��  $	 3��5]     $
 True��5^     
 5_     P� 5`      �@J�IsMouseOver$
 true��6=   . �5a  "   �� $ 	rectangle��6)   8   � ��6V   5b     P� 5c      �@n�
IsDragging$
 true��6<   . �5d  "   �� $ 	rectangle��6)   8   � ��6V   5e     5g     5i     5j     �� 5l  
   " ��6,   .�5m     ��  � ��8   6+   �� 5n      � ��8  6,   �� 5o      �@��IsPressAndHoldEnabled$ false��6=   �� 5p      �@��IsFlicksEnabled$ false��67   �� 5q      � ��8  6+   �� 5r      � ��$ 10��6&   �� 5s      � ��$ 10��6)   �� 5t      � ��$ 10��6'   �� 5u      � ��$ 10��6*   �� 5v      � �� 5w     �� 5x     " ��6&   .R�5y     � $ Bg��5z     #��z��5{     $
 true��5|     .+�5}     ]� $) 
PART_Track��5~  !   $
��true��5  !   #{�z* 5�  !   + 5�  "   �� 5�  &   #��Z, 63   $y�	Collapsed=�6`   - 5�  "   �� 5�  &   #��Z. 63   $y�	Collapsed=�6b   / 5�  "   n� 5�  &   8�� 6,   5�     5�     5�     5�      5�     P� 5�      �@��Orientation$ 
Horizontal=�61   . �5�     ��  � ��$
 Auto��6.   �� 5�      � ��$ 0��61   �� 5�      � ��$ 10��6/   �� 5�      � ��$ 10��62   �� 5�      � �� 5�     �� 5�     " ��6.   .R�5�  "   � $ Bg��5�  %   #��z��5�  %   $
 true��5�  %   .+�5�  &   ]� $) 
PART_Track��6,   #{�z* 6@   + 5�  *   �� 5�  .   #��Z0 6;   $y�	Collapsed=�6j   - 5�  *   �� 5�  .   #��Z1 6;   $y�	Collapsed=�6k   / 5�  *   n� 5�  .   8�� 64   5�  '   5�  #   5�     5�     5�     5�     �� 5�  
   " ��6/   .�5�     ��  � ��
 ���� 6+   �� 5�      � ��
 ���� 6,   �� 5�      � ��$ 0q�60   �� 5�      � ��$ 0q�6(   �� 5�      � |�$ false��6*   �� 5�      � ��$
 Auto=�6>   �� 5�      � ��$
 Auto=�6<   �� 5�      � �� 5�     �� 5�     " ��6&   .R�5�     �� #��z��5�     #��z��5�     #��z��5�     $
 True��5�     ._�5�     � .+�5�  "   �� $!2 PART_ScrollContentPresenter��5�  %   #��z��5�  %   #3 z��5�  %   $
4 True��5�  %   $5 False��5�  %   #��z��5�  %   #��z��5�  %   �� 5�  "   $6 PART_VerticalScrollBar��5�  %   $��Right=�5�  %   $7 VerticalScrollBar��5�  %   $8 Arrow��5�  %   #: z9 5�  %   $; 0Q�5�  %   8��  #= z< 5�  %   #y�z> 5�  %   ? 5�  %   �� *VerticalOffset+$! OneWay=� �� *TemplatedParent+�� 5�  "   $6 PART_HorizontalScrollBar��5�  %   $��Bottom=�5�  %   $7 HorizontalScrollBar��5�  %   $8 Arrow��5�  %   #: z@ 5�  %   $; 0Q�5�  %   $A 
Horizontal=�5�  %   8�� #= zB 5�  %   #y�zC 5�  %   ? 5�  %   �� *HorizontalOffset+$! OneWay=� �� *TemplatedParent+5�     5�     5�     5�     5�     �� 5�  
   " ��6,   .�5�     ��  � ��
 ���� 6+   �� 5�      � ��8   6+   �� 5�      � ��$ 0q�60   �� 5�      � ��$ 42(�6)   �� 5�      � ��$
 Bold"�6+   �� 5�      � ��$
 10,0q�6(   �� 5�      � ��$ 0q�6'   �� 5�      � ��$ Center=�62   �� 5�      � ��$
 Left=�64   �� 5�      �@�Cursor$
 Hand��6'   �� 5�      � �� 5�     �� 5�     " ��6&   .R�5�     �� #( z��6!   #��z��6E   ._�5�     �� $��Center=�5�  !   $��Center=�5�  !   #��z��5�  !   #��z��5�  !   #��z��5�  !   $	��←��5�  !   5�     
 5�     P� 5�      �@J�IsMouseOver$
 True��6=   . �5�  "   �� 8   � ��6?   5      P� 5      � {�$ False��6;   . �5  "   ��  �@J�Opacity$	 0.5Q�6<   �� 5  "    � �� �#8886?   5     P� 5      � ��$
 True��6;   . �5  "   �� 8   � ��6?   5     5	     5     5     �� 5  
   " ��6'   .�5     ��  � ��8   6+   �� 5      � ��$ 14(�6)   �� 5      � ��$ 
10,-8,10,8q�6'   �� 5      � ��$
 Wrap=�6-   5     5     .��5     � #���   6   .+�5  
   � D � RowDefinitionsD 5     �� 5     $
��Auto �6    �� 5     $��* �6    .+�5     � $��0��6   $��15,10,15,15q�6     5     �� 5     $
��Auto �6'   �� 5     $��* �6'   .+�5!     �� $��0��5"     #��[ 5%     ��5#     �� *GoBackCommand+��5$     ��  �� *FindAncestor+E �� AncestorType"E N�5%     �� 5&     $��1��5'     $��15,0,0,0q�5(     $��Center=�5)     $��Center=�5*     $��30(�5+     $��SemiBold"�5,     #��[ 5-       AddFilmPage_Title#���  5.     5/     �� 50     $��1��51     $	��800��52     $
��1100��53     $	��650��54     $( 20q�55     $��Center=�56     $	��Top=�57     #��[ 58     $	 5��59     ._�5:     � D 5;     �� 5<     $��* �6(   �� 5=     $
��Auto �6(   .+�5?     �� $��0��5@     $��0,0,5,5q�5A     $��Disabled=�5B     #��[ 5C     $
��Auto=�5D     .��5E     �  5F     �� 5G  "   $
��1.2* �63   $	��250��6@   �� 5H  "   $
��1.5* �63   $	��280��6@   �� 5I  "   $��1* �63   $	��250��6>   .+�5L     �� $��0��5M  !   $��0,0,10,0q�5N  !   $	��Top=�5O  !   .+�5P  "   �� #��[ 6,     AddFilmPage_Label_Title#���  6U   �� 5Q  "   #��[ 6*   ��6U   �� *Title+F �� UpdateSourceTrigger$F PropertyChanged=�"G �� ValidatesOnNotifyDataErrors$G False���� 5R  "   #��[ 5S  %   ��5T  %   �� *
TitleError+y�5U  %   �� *IsTitleErrorVisible+H �� 	Converter#H [	 �� 5V  "   #��[ 6,     AddFilmPage_Label_Director#���  6U   �� 5W  "   #��[ 6*   ��6U   �� *
Director+$F PropertyChanged=�$G False���� 5X  "   #��[ 5Y  %   ��5Z  %   �� *DirectorError+y�5[  %   �� *IsDirectorErrorVisible+#H [	 �� 5\  "   #��[ 6,     AddFilmPage_Label_Genre#���  6U   �� 5]  "   #��[ 5`  %   ��5^  %   �� *AvailableGenres+I �� SelectedValueI 5_  %   �� *SelectedGenre+$! TwoWay=�$F PropertyChanged=�5`  %   �� 5a  "   #��[ 5b  %   ��5c  %   �� *
GenreError+y�5d  %   �� *IsGenreErrorVisible+#H [	 �� 5e  "   #��[ 6,     AddFilmPage_Label_Duration#���  6U   �� 5f  "   #��[ 6*   ��6U   �� *
Duration+$F PropertyChanged=�$G False���� 5g  "   #��[ 5h  %   ��5i  %   �� *DurationError+y�5j  %   �� *IsDurationErrorVisible+#H [	 �� 5k  "   #��[ 6,     AddFilmPage_Label_Rating#���  6U   �� 5l  "   #��[ 5o  %   ��5m  %   �� *AllowedRatings+I 5n  %   �� *Rating+$! TwoWay=�$F PropertyChanged=�5o  %   �� 5p  "   #��[ 5q  %   ��5r  %   �� *RatingError+y�5s  %   �� *IsRatingErrorVisible+#H [	 5t     �� 5v     $��1��5w  !   $��	10,0,10,0q�5x  !   $��Stretch=�5y  !   .+�5z  "   �� $��
10,10,10,5q�5{  %   $	��Top=�5|  %   #��[ 5}  %    ! AddFilmPage_Label_Description#���  5~  %   �� 5  "   $��Stretch=�5�  %   #��[ 5�  %   ��5�  %   �� *Description+$F PropertyChanged=�$G False���� 5�  "   $��
10,-1,10,8q�5�  %   #��[ 5�  %   ��5�  %   �� *DescriptionError+y�5�  %   �� *IsDescriptionErrorVisible+#H [	 5�     � 5�     $��2��5�  !   $��5,0,0,0q�5�  !   $	��Top=�5�  !   D 5�  "   �� 5�  &   $
��Auto �64   �� 5�  &   $��* �64   $	��300��6?   �� 5�  &   $
��Auto �64   .+�5�  "   �� $��0��5�  %   $��	0,10,0,10q�5�  %   $��Center=�5�  %   J �� Orientation$J 
Horizontal=�5�  %   .+�5�  &   ��   AddFilmPage_Button_AddPhoto#���  5�  )   #��[
 5�  )   ��5�  )   �� *SelectImageCommand+5�  )   �� 5�  &    " AddFilmPage_Button_RemovePhoto#���  5�  )   #��[
 5�  )   ��5�  )   �� *RemoveImageCommand+{�5�  )   �� *IsRemovePhotoEnabled+5�  )   5�  #   �� 5�  "   $��1��5�  %   #��[ 5�  %   #��[ 5�  %   $��1q�5�  %   $	 3��5�  %   ._�5�  &   �� #��[ 6R   ��6,   �� *PreviewImageSource+6R   5�  #   �� 5�  "   $��2��5�  %   $��	10,5,10,0q�5�  %   #��[ 5�  %   ��5�  %   �� *
ImageError+y�5�  %   �� *IsImageErrorVisible+#H [	 5�     5�     5�     �� 5�     $��1��5�     $��0,20,0,0q�5�     $��Center=�5�     $J 
Horizontal=�5�     .+�5�     ��   AddFilmPage_Button_AddFilm#���  5�     K �� 	IsDefault$
K True��5�     #��[
 5�     ��5�     �� *AddFilmCommand+5�     �� 5�     $��15,5,5,5q�5�         AddFilmPage_Button_ClearForm#���   5�     #��[
 5�     ��5�     �� *ClearFormCommand+5�     5�     5�     5�     5�     5�     5�     