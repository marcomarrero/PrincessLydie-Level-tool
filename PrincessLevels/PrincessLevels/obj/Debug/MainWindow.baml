   M S B A M L   `   `   `  ����I  EPrincessLevels, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:PrincessLevelsPrincessLevels  5   	       PrincessLevels.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:PrincessLevels     5   	     � Title$$  Princess Lydie Level Generator��5   	   $��373.077��60   $��875.692��6A   .��5	      � 
�������$	��346��6   $��Bottom=�6+    � ColumnDefinitions 5
   
   �� 5      $	��79* �6   �� 5      $
��124* �6   �� 5      $	��47* �6   .+�5   
   �� -   5         ��Name$	 pic��5      $
��Left=�6   $	��260��68   $��	10,69,0,0q�6E   $	��Top=�6X   $	��386��6p    � Cursor$ Cross��6|   $��2��6�   �� 5   
   -   5         ��Name$ Filename��5      $��33��6$   $��	10,10,0,0q�60    �� TextWrapping$
 Wrap=�6C   $	��Top=�6W   $��2��6o   $
��Left=�6�   $	��425��6�   �� 5   
   -   5         ��Name$ ButtonLoadImage��5      $��	Load File��6*   $
��Left=�6>   $��33��6Y   $��
253,10,0,0q�6e   $	��Top=�6y   $��89��6�   $��1��6�   �� 5   
   -   5         ��Name$ ListBoxCode��5      $��166,83,10,41q�6'   $ IBeam��6=    � ForceCursor . $��1��6_   $��2��6o   �� 5   
   $��Image: (must be 20x12)��6   $
��Left=�61   $��26��6L   $��	10,43,0,0q�6X   $	��Top=�6k   $	��313��6�   	 J� RenderTransformOrigin$	 	0.567,0.3&�6�   $��2��6�   �� 5   
   $��IntyBasic Source Code:��6   $
��Left=�61   $��36��6L   $��
166,48,0,0q�6X   $	��Top=�6l   $	��152��6�   $��1��6�   $	 0.542,-6.732&�6�   $��Bottom=�6�   �� 5   
   -   5        $ ButtonGenerate��5      $��Process Loaded File��6)   $
��Left=�6G   $��33��6b   $��
347,10,0,0q�6n   $	��Top=�6�   $	��117��6�   $��1��6�   $��2��6�   �� 5   
   -   5        $ ButtonSelect��5      $��Select File...��6'   $
��Left=�6@   $��33��6[   $��
166,10,0,0q�6g   $	��Top=�6{   $��82��6�   $��1��6�   �� 5   
   -   5        $ 
ButtonCopy��5      $��Copy to Clipboard��6%   $
��Left=�6A   $��
166,0,0,10q�6\   $	��152��6p   $��1��6�   $��26��6�   $��Bottom=�6�   �� 5   
   -   5        
 ��Name$
 Check8��5      $��
8-bit data��6#   $
��Left=�68   $��14��6S   $��	60,64,0,0q�6_   $	��Top=�6r   $��84��6�   $��2��6�   �� 5   
   -	   5        $ 	ButtonAll��5      $��Process All Files��6$   $��2��6@   $
��Left=�6P   $��33��6k   $��	38,10,0,0q�6w   $	��Top=�6�   $	��117��6�   6�   �� 5   
   -
   5        $ ButtonClear��5      $��Clear text...��6&   $��2��6>   $
��Left=�6N   $��	10,0,0,10q�6i   $	��143��6|   $	 0.503,0.864&�6�   $��26��6�   $��Bottom=�6�   5      5      