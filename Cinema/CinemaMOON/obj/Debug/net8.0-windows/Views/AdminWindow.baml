   M S B A M L   `   `   `  ����E  ACinemaMOON, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:CinemaMOON.ViewsCinemaMOON.Views  5      =#clr-namespace:CinemaMOON.ViewModelsCinemaMOON.ViewModels  5      "    CinemaMOON.Views.AdminWindow   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:CinemaMOON.Views     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      2vm#clr-namespace:CinemaMOON.ViewModels     5        � Title$  КИНОТЕАТР MOON��5	      $
��1200��5
      $	��900��5      
���   � � WindowStartupLocation$ CenterScreen=�5       � WindowStyle$
 None=�5      '  !System.Windows.Shell.WindowChrome   WindowChrome 5        5   
      CaptionHeight$ 0Q�5         CornerRadius$ 0��5         GlassFrameThickness$ 0q�5       � 	Resources 5         TitleBarButtonStyle �� 
TargetType	 �� Property
 �� Value �� 
TargetType ��Name �� Triggers P� Property P� Value �� 
TargetName  CloseButtonStyle �� BasedOn �� Triggers  TransparentButtonStyle  GoldButtonStyle  NavButtonStyle �� Padding  LogoButtonStyle��%�  &	        &	     7   '�� ]    &	 �    &	 '    &	 �	    &	 -    �� 5   
   " ��6,   .�5      �� 	 � ��
 5      �� 5      " ��6&   .R�5      �� $ bdr_main��5      
������ 
������ $��1q�5!      ._�5"      �� $��Center=�5#   !   $��Center=�5$   !   $��Content��5%   !   5&       5'      P� 5(       �@J�IsMouseOver$
 True��6=   . �5)   "   �� $ bdr_main��6)   	 � ��
 �#4747476U   5*      P� 5+       � ��$
 True��6;   . �5,   "   �� $ bdr_main��6)   	 � ��
 �#4747476U   5-      5/      51      52      �� 54   
   8   " ��57      .�58      �� 	 � ��
 59      �� 5:      " ��6&   .R�5;      �� $ bdr_main��5<      
������ 
������ $��1q�5?      ._�5@      �� $��Center=�5A   !   $��Center=�5B   !   $��Content��5C   !   5D       5E      P� 5F       �@J�IsMouseOver$
 True��6=   . �5G   "   �� $ bdr_main��6)   	 � ��
 �#FF00006U   �� 5H   "   	 � ��

 �����6?   5I      P� 5J       � ��$
 True��6;   . �5K   "   �� $ bdr_main��6)   	 � ��
 �#CC00006U   5L      5N      5P      5Q      �� 5S   
   " ��6   .�5T      �� 	 � ��

 �����6+   �� 5U      	 � ��

 ���� 6+   �� 5V      	 � ��
 �#D7B4726,   �� 5W      	 � ��$
 0,0,0,1q�60   �� 5X      	 � ��$
 5q�6(   �� 5Y      	 � ��$
 16(�6)   �� 5Z      	 �@��
CaretBrush
 �#D7B4726+    5[      P� 5\       �@J�IsMouseOver$
 True��61   . �5]      �� 	 �@�Cursor$
 IBeam��6/   �� 5^      	 � ��
 �#FFD70064   5_      P� 5`       �@J�	IsFocused$
 True��6/   . �5a      �� 	 � ��
 �#FFD70064   5b      5d      �� 5f   
   " ��6/   .�5g      �� 	 � ��

 ���� 6+   �� 5h      	 � ��
 �#D7B4726,   �� 5i      	 � ��
 �#D7B4726+   �� 5j      	 � ��$
 1q�60   �� 5k      	 � ��$

 10,5q�6(   �� 5l      	 � ��$
 12(�6)    5m      P� 5n       �@J�IsMouseOver$
 True��61   . �5o      �� 	 � ��
 �	#20D7B47263   5p      5r      �� 5t   
   " ��6(   .�5u      �� 	 � ��
 �#D7B4726+   �� 5v      	 � ��
 �#D7B4726,   �� 5w      	 � ��

 �   �6+   �� 5x      	 � ��$
 1q�60   �� 5y      	 � ��$

 10,5q�6(   �� 5z      	 � ��$
 12(�6)    5{      P� 5|       �@J�IsMouseOver$
 True��61   . �5}      �� 	 � ��
 �#FFD70063   5~      5�      �� 5�   
   " ��6'   .�5�      �� 	 � ��$
 0q�60   �� 5�      	 � ��

 ���� 6+   �� 5�      	 � ��$

 15,5q�6(   �� 5�      	 � ��$
 26(�6)   �� 5�      	 � ��
 �#D7B4726+   �� 5�      	 �@�Cursor$

 Hand��6'   �� 5�      	 � ��$
 Center=�69   �� 5�      	 � ��$
 Center=�6;   �� 5�      	 � ��
 5�      �� 5�      " ��6&   .R�5�      �� # z��5�      #��z��5�      #��z��5�      #��z��5�      ._�5�      �� #��z��6/   #��z��6r   5�       5�      P� 5�       �@J�IsMouseOver$
 True��6=   . �5�   "   �� 	 � ��
 �#FFD7006?   5�      P� 5�       � ��$
 True��6;   . �5�   "   �� 	 � ��
 �#C6A35D6?   5�      P� 5�       � {�$ False��6;   . �5�   "   �� 	 � ��
 �#8886?   �� 5�   "   	 �@J�Opacity$	
 0.6Q�6<   5�      5�      5�      5�      �� 5�   
   " ��6(   .�5�      �� 	 � ��

 ���� 6+   �� 5�      	 � ��

 ���� 6,   �� 5�      	 � ��$
 0q�60   �� 5�      	 � ��$
 0q�6(   �� 5�      	 � ��$
 Center=�62   �� 5�      	 �@�Cursor$

 Hand��6'   �� 5�      	 � ��
 5�      �� 5�      " ��6&   .R�5�      �� # z��5�      #��z��5�      #��z��5�      #��z��5�      ._�5�      �� $��Center=�6/   $��Center=�6L   5�      5�      5�       5�      P� 5�       �@J�IsMouseOver$
 True��61   5�      5�      .��5�      �  � RowDefinitions 5�   
   �� 5�      $��40 �6   �� 5�      $	��100 �6   �� 5�      $��* �6   .+�5�   
   � -   5        $��0��5�      
���   �.+�5�      � $
��Left=�6   $��Center=�63   .+�5�      �� $��30��5�      $��5,0,0,0q�5�      $,��&/Resources/Images/IconForCinemaApp.png��5�      5�      �� 5�      $��Right=�6    �� Orientation$ 
Horizontal=�65   .+�5�      �� -   5         ��Name$ buttonMinimise��5�      $��30��5�      
������ 
������ #��[  5�      .��5�      �� $��20��5�      $��20��5�      $*��$/Resources/Images/MinimizeWindow.png��5�      5�      �� 5�      -   5        $ buttonMaximise��5�      $��30��5�      
������ 
������ #��[  5�      .��5�      �� -   5         ��Name$ maximizeImage��5�      $��20��5�      $��20��5�      $*��$/Resources/Images/MaximizeWindow.png��5�      5�      �� 5�      -   5        $ buttonClose��5�      $��30��5�      
������ 
������ #��[ 5�      .��5�      �� $��20��5�      $��20��5�      $'��!/Resources/Images/CloseWindow.png��5�      5�      5�      5�      �� 5�   
   $��1��5�        PageBackgroundColor#���  5�      ���#D7B472$��0,2,0,2q�5     ._�5     �  � ColumnDefinitions 5     �� 5     $
��Auto �6'   �� 5     $��* �6'   .+�5     �� -   5        $��
logoButton��5	     $��0��5
     $
��20,0q�5     $
��Home��5     #��[ 5     ��5     �� *NavigateCommand+.��5     �� $	��200��5     $��60��5     $��Center=�5     $+��%/Resources/Images/IconForMainPage.png��5     5     �� 5     $��1��5     $��0,0,20,0q�5     $��Right=�5     $��Center=�5     $ 
Horizontal=�5     .+�5     �� -   5        $��AfishaButton��5     $��Poster��5        	NavAfisha#���  5!     #��[ 5"     ��5     �� *NavigateCommand+5"     �� 5$     -   5        $��HallsButton��5%     $
��15,0q�5&     $��Halls��5(       NavHalls#���  5)     #��[ 5*     ��5'     �� *NavigateCommand+5*     �� 5,     -	   5        $��AboutUsButton��5-     $
��15,0q�5.     $��AboutUs��50      	 
NavAboutUs#��� 	 51     #��[ 52     ��5/     �� *NavigateCommand+52     �� 54     -
   5        $��AccountButton��55     $
��15,0q�56     $��
AdminPanel��58      
 NavAdminPanel#��� 
 59     #��[ 5:     ��57     �� *NavigateCommand+5:     5;     5<     5=     �� 5?  
   $��2��6   #���  6   ._�5@      � -   5        $��frameMainForAdmin��5A     
������   � NavigationUIVisibility$ Hidden=�5C     5D     5E     5F     