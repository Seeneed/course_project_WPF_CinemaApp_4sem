   M S B A M L   `   `   `  ����E  ACinemaMOON, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:CinemaMOON.ConvertersCinemaMOON.Converters  5      3clr-namespace:CinemaMOON.ViewsCinemaMOON.Views  5      =#clr-namespace:CinemaMOON.ViewModelsCinemaMOON.ViewModels  5	      $    CinemaMOON.Views.RateMoviePage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      :
converters#clr-namespace:CinemaMOON.Converters     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:CinemaMOON.Views     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      2vm#clr-namespace:CinemaMOON.ViewModels     5	         RateMoviePage_Title  N� Title#  �   5
        PageBackgroundColor N� 
Background# �  5       � 	Resources 5        GoldAccentBrush �� Color  BrightGoldAccentBrush  PrimaryTextBrush  DarkElementBackgroundBrush  SlightlyLighterDarkBrush  ErrorTextBrush8   2CinemaMOON.Converters.BooleanToVisibilityConverter   BooleanToVisibilityConverter 	 PageBackButtonStyle �� 
TargetType �� Property �� Value �� 
TargetType �� Padding	 �� Triggers
 P� Property P� Value 
 RatingLabelStyle  RatingInputStyle ��Name �� CornerRadius ��Name �� 
TargetName �� Value �� Binding �� Path  RatingErrorTextStyle  RatingSubmitButtonStyle��%�  &	       &	 !     &	 B     &	 c     &	 �     &	 �     &	 �     &		 �     7  7  7  &	
 �    7  &	 �    7  7  7  7  7  7  7  &	 
    7  &	 �
    7  7  7  7  7  7  7  7  �� 5   
   $ #D7B472��62   �� 5   
   $ #FFD700��68   �� 5   
   $ #E0E0E0��63   �� 5   
   $ #1C1C1C��6=   �� 5   
   $ #2A2A2A��6;   �� 5   
   $ #FF6B6B��61     5   
   62   �� 5   
   " ��6,   .�5      ��  � ��
 ���� 6+   �� 5       � ��8   6+   �� 5       � ��$ 0q�60   �� 5       � ��$ 42(�6)   �� 5       � ��$
 Bold"�6+   �� 5        � ��$
 10,0q�6(   �� 5!       � ��$ 0q�6'   �� 5"       � ��$ Center=�62   �� 5#       � ��$
 Left=�64   �� 5$       �@�Cursor$
 Hand��6'   �� 5%       � �� 5&      �� 5'      " ��6&   .R�5(      �� # z��6!   #��z��6E   ._�5)      �� $��Center=�5*   !   $��Center=�5+   !   #��z��5,   !   #��z��5-   !   #��z��5.   !   $	��←��5/   !   50      	 51      P� 52      
 �@J�IsMouseOver$
 True��6=   . �53   "   �� 8   � ��6?   54      P� 55      
 � {�$ False��6;   . �56   "   ��  �@J�Opacity$	 0.5Q�6<   �� 57   "    � �� �#8886?   58      P� 59      
 � ��$
 True��6;   . �5:   "   �� 8   � ��6?   5;      5=      5?      5@      �� 5B   
   " ��6)   .�5C      ��  � ��8   6+   �� 5D       � ��$ Center=�62   �� 5E       � ��$ 
10,15,10,2q�6'   �� 5F       � ��$ 22(�6)   �� 5G       � ��$ SemiBold"�6+   5H      �� 5J   
   " ��6)   .�5K      ��  � ��8   6+   �� 5L       � ��8  6+   �� 5M       �@��
CaretBrush8  6+   �� 5N       � ��8  6,   �� 5O       � ��$ 1q�60   �� 5P       � ��$	 8,5q�6(   �� 5Q       � ��$ 	10,0,10,5q�6'   �� 5R       � ��$ 34��6*   �� 5S       � ��$ 20(�6)   �� 5T       � ��$ Center=�69   �� 5U       �@=�ErrorTemplate 69   Z� �� 5V       � �� 5W      �� 5X      " ��6&   .R�5Y      �� $ Bd��5Z      #��z��5[      #��z��5\      #��z��5]      $ 3��5^      ._�5_      �� $ PART_ContentHost��5`   !   #��z��5a   !   #��z��5b   !   $|�false��5c   !   5d      	 5e      P� 5f      
 �@J�IsMouseOver$
 True��6=   . �5g   "   �� $ Bd��6)   8   � ��6P   5h      P� 5i      
 �@J�	IsFocused$
 True��6;   . �5j   "   �� $ Bd��6)   8   � ��6P   �� 5k   "   $ Bd��6)    � ��$ 1q�6T   5l      �� 5m      $
 True��6X    6*   �� $ IsRatingErrorVisible�.a�5n   "   �� $ Bd��6)   8   � ��6P   5o      5q      5s      5t      �� 5v   
   " ��6-   .�5w      ��  � ��8   6+   �� 5x       � ��$ 18(�6)   �� 5y       � ��$ 	12,0,10,5q�6'   �� 5z       � ��$
 Wrap=�6-   5{      �� 5}   
   " ��60   .�5~      ��  � ��8   6+   �� 5       � ��8  6+   �� 5�       � ��$ 1q�60   �� 5�       � ��8  6,   �� 5�       � ��$ 20,10q�6(   �� 5�       � ��$ 0,25,0,0q�6'   �� 5�       � ��$	 250��6)   �� 5�       � ��$ 40��6'   �� 5�       � ��$ 20(�6)   �� 5�       � ��$ SemiBold"�6+   �� 5�       �@�Cursor$
 Hand��6'   �� 5�       � ��$ Center=�64   �� 5�       � �� 5�      �� 5�      " ��6&   .R�5�      �� $ border��5�      #��z��5�      #��z��5�      #��z��5�      $ 3��5�      ._�5�      �� $��Center=�6/   $��Center=�6L   5�      	 5�      P� 5�      
 �@J�IsMouseOver$
 True��6=   . �5�   "   �� $ border��6)   8   � ��6S   �� 5�   "   8   � ��6@   �� 5�   "   8   � ��6?   5�      P� 5�      
 � ��$
 True��6;   . �5�   "   �� $ border��6)    � �� �#3535356S   �� 5�   "   8   � ��6@   �� 5�   "   8   � ��6?   5�      P� 5�      
 � {�$ False��6;   . �5�   "   ��  �@J�Opacity$	 0.5Q�6<   �� 5�   "   $ border��6)    � �� �#3838386S   �� 5�   "    � �� �#5556@   �� 5�   "    � �� �#9996?   �� 5�   "    �@�Cursor$ No��6;   5�      5�      5�      5�      5�      .��5�      �  � RowDefinitions 5�   
   �� 5�      $
��Auto �6   �� 5�      $��* �6   .+�5�   
   � $��0��6   $��15,10,15,15q�6    � ColumnDefinitions 5�      �� 5�      $
��Auto �6#   �� 5�      $��* �6#   .+�5�      �� $��0��5�      #��[	 5�      ��5�      �� *GoBackCommand+��5�      ��  �� RelativeSource ��  �� AncestorType Page5�      �� 5�      $��1��5�      $��15,0,0,0q�5�      $��Center=�5�      $��Center=�5�      $��30(�5�      $��SemiBold"�5�      #��[ 5�      #���   5�      5�      �� 5�   
   $��1��5�      $	��600��5�      $ 30q�5�      $��Center=�5�      $��Center=�5�      #��[ 5�      #��[ 5�      $��1q�5�      $ 5��5�      ._�5�      �� $��Center=�6   .+�5�      �� #��[
 6    ! RateMoviePage_MovieTitleLabel#���  6F   �� 5�      $��
10,0,10,15q�5�      $��20(�5�      #��[ 5�      $
��Wrap=�5�      ��5�      �� *
MovieTitle+5�      �� 5�      #��[
 6    ! RateMoviePage_YourRatingLabel#���  6F   �� 5�       �� 	MaxLength$ 2��5�      #��[ 5�      ��5�      �� *RatingInputString+ �� UpdateSourceTrigger$ PropertyChanged=��� 5�      #��[ 5�      ��5�      �� *RatingError+y�5�      �� *IsRatingErrorVisible+ �� 	Converter# [ �� 5�      $��
10,10,10,0q�5�      $��18(�5�      #��[ 5�      ��5�      �� *AlreadyRatedText+y�5�      �� *IsAlreadyRated+# [ �� 5�       $  RateMoviePage_SubmitRatingButton#���  5�      #��[ 5�      ��5�      �� *SubmitRatingCommand+{�5�      �� *CanSubmitRating+5�      5�      5�      5�      5�      