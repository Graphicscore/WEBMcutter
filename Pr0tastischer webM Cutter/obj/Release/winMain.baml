   M S B A M L   `   `   `  ���� T  PPr0tastischer webM Cutter, Version=2.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:Pr0tastischer_webM_CutterPr0tastischer_webM_Cutter  *    $Pr0tastischer_webM_Cutter.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     9local'clr-namespace:Pr0tastischer_webM_Cutter       � Title$  
MainWindow��$	��450��$
��1000�����	#FF161618���	#FF1DB992 � Icon$ logoPr0tastisch.ico�� � WindowStartupLocation$ CenterScreen=�$
��1000��$	��450��.��� -    �Name$ mainGrid��.+�� -   $ gridContent�� J� Effect -  'System.Windows.Media.Effects.BlurEffect  -   gridContentBlurEffekt��   Radius$ 0Q�.+��� -    ��Name$ 	bVorschau��
���   �$��1,1,1,1q�$��10,10,463,140q� �� CornerRadius$ 5,5,5,5��
���   �._�� .+��� -    ��Name$ imgVorschau��$��0,0,0,0q�   -   VorschauBlurEffekt��$ 0Q�	 J� 	CacheMode	 &   System.Windows.Media.BitmapCache  � -   $ gridVorschauBildEffekt��
���   �
 J� Opacity$	
 0.6Q�$y�	Collapsed=��� -    ��Name$ labInfVorschau��$ ��Wähle einen Startpunkt...��$��Right=�$��Bottom=����	#FF1DB992$	��579��$��Right=�$��10.667(�$
��Bold"� 3  -System.Windows.Media.Effects.DropShadowEffect     ShadowDepth$ 0Q�   
BlurRadius$ 2Q�� $	��125��$��10,0,463,10q�$��Bottom=�.+��� $��Startzeitpunkt��$��
10,10,10,0q�$	��Top=����	#FF1DB992$��Center=��� $��Laufzeit��$��
10,64,10,0q�$	��Top=����	#FF1DB992$��Center=��� -	   $ labInfStart��$��00:00:00.000��$��	0,10,10,0q�$	��Top=����	#FF1DB992$��Right=�$��10.667(� � Cursor$
 Hand��$��41��$��Bottom=�$��Right=�$��74���� -
   $ labInfLaenge��$��00:00:00.000��$��	0,64,10,0q�$	��Top=����	#FF1DB992$��Right=�$��10.667(�$
 Hand��$��42��$��Bottom=�$��Right=�$��74���� -    ��Name$ 
sstartzeit��$��
10,43,10,0q�$	��Top=� � SmallChange$
 1000Q� � LargeChange$ 60000Q��� -   $ slaenge��$��
10,97,10,0q�$	��Top=�$	 100Q�$
 1000Q� � Maximum$ 120000Q�� $��0,10,10,140q�$��Right=�$	��448��.+��� .���� .+�9�  9� 
IsExpanded . $��Einstellungen�����	#FF1DB992.��� $	��300��.+��� -   $ sqmin��$
��Left=�$��	62,10,0,0q�$	��Top=�$	��299��$ 63Q� � Minimum$ 1Q�$ 1Q� � Value$ 10Q�$ 3Q��� -   $ sqmax��$
��Left=�$��	62,31,0,0q�$	��Top=�$	��299�� J� RenderTransformOrigin$ 0.308,-0.322&�$ 1Q�$ 10Q�$ 63Q�$ 35Q�$ 3Q��� -   $
 scrf��$
��Left=�$��	62,52,0,0q�$	��Top=�$	��299��$ 1Q�$ 3Q�$ 63Q�$ 19Q�$ 1Q��� -   $ sbreite��$
��Left=�$��	62,86,0,0q�$	��Top=�$	��299��$ 10Q�$	 100Q�$ -1Q�$
 1920Q�$	 720Q� �� TickFrequency$ 10Q��� -   $ shoehe��$
��Left=�$��
62,107,0,0q�$	��Top=�$	��299��$ -1Q�$ -1Q�$ 10Q�$
 1080Q�$ 10Q�$ 10Q��� -   $ scropbreite��$
��Left=�$��
62,141,0,0q�$	��Top=�$	��299��$
 1920Q�$ 1Q�$ 10Q��� -   $ 
scrophoehe��$
��Left=�$��
62,162,0,0q�$	��Top=�$	��299��$
 1080Q�$ 10Q�$ 1Q��� -   $ scropx��$
��Left=�$��
62,183,0,0q�$	��Top=�$	��299��$
 1920Q�$ 10Q�$ 1Q��� -   $ scropy��$
��Left=�$��
62,204,0,0q�$	��Top=�$	��299��$
 1080Q�$ 10Q�$ 1Q��� -   $ 
labInfQMin��$��10��$��Right=�$��0,5,0,0q�$	��Top=����	#FF1DB992$��10.667(�$
 Hand��$��41���� -   $ 
labInfQMax��$��35��$��Right=�$��0,26,0,0q�$	��Top=����	#FF1DB992$��10.667(�$
 Hand��$��41���� -   $ 	labInfCRF��$��19��$��Right=�$��0,47,0,0q�$	��Top=����	#FF1DB992$��10.667(�$
 Hand��$��41���� -   $ labInfBreite��$	��720��$��Right=�$��0,81,0,0q�$	��Top=����	#FF1DB992$��10.667(�$
 Hand��$��41���� -   $ labInfHoehe��$��-1��$��Right=�$��	0,102,0,0q�$	��Top=����	#FF1DB992$��10.667(�$
 Hand��$��41���� -    ��Name$ 
cbAudioAus��$��Audiospur deaktivieren��$
��Left=�$��
62,266,0,0q�$	��Top=�$	��299�� d� 	IsChecked$
 TrueY��� $
��Qmin��$
��Left=�$��10,5,0,0q�$	��Top=����	#FFBABABA$��10.667(�$��47�� � ToolTip$ minimum Quantizer��$��Right=��� $
��Qmax��$
��Left=�$��	10,26,0,0q�$	��Top=����	#FFBABABA$��10.667(�$��47��$ maximum Quantizer��$��Right=��� $	��CRF��$
��Left=�$��	10,47,0,0q�$	��Top=����	#FFBABABA$��10.667(�$��47��$ Constant Rate Factor��$��Right=��� $��Breite��$
��Left=�$��	10,81,0,0q�$	��Top=����	#FFBABABA$��10.667(�$��47��$< 6Ausgabe Breite des Videos (-1 für relativ zur Höhe).��$��Right=��� $��Höhe��$
��Left=�$��
10,102,0,0q�$	��Top=����	#FFBABABA$��10.667(�$��47��$< 6Ausgabe Höhe des Videos (-1 für relativ zur Breite).��$��Right=��� $��Breite��$
��Left=�$��
10,136,0,0q�$	��Top=����	#FFBABABA$��10.667(�$��47��$) #Beschneide das Video in der Breite.��$��Right=��� $��	Qualität��$
��Left=�$��
-20,27,0,0q�$	��Top=����	#FFBABABA$��10.667(�$��59��$" Allgemein: kleiner = besser.��$��Center=�$ 0.5,0.5&�z�V� .~��� �� ��  �� Angle$	 270Q�T� �� $��
Auflösung��$
��Left=�$��
-20,94,0,0q�$	��Top=����	#FFBABABA$��10.667(�$��59��$2 ,Skaliere das Video auf eine neue Auflösung.��$��Center=�$ 0.5,0.5&�z�V� .~��� �� �� $	 270Q�T� �� $��Beschneiden��$
��Left=�$��-35,169,0,0q�$	��Top=����	#FFBABABA$��10.667(�$��87��$G ADas ist doch einfach nur scheiße... Zum deaktivieren alle auf 0.��$��Center=�$ 0.5,0.5&�z�V� .~��� �� �� $	 270Q�T� �� $��Höhe��$
��Left=�$��
10,157,0,0q�$	��Top=����	#FFBABABA$��10.667(�$��47��$( "Beschneide das Video in der Höhe.��$��Right=��� $��X-Pos.��$
��Left=�$��
10,178,0,0q�$	��Top=����	#FFBABABA$��10.667(�$��47��$! Lege die X-Koordinate fest.��$��Right=��� $��Y-Pos.��$
��Left=�$��
10,199,0,0q�$	��Top=����	#FFBABABA$��10.667(�$��47��$! Lege die Y-Koordinate fest.��$��Right=��� -   $ labInfCropBreite��$��0��$��Right=�$��	0,136,0,0q�$	��Top=����	#FF1DB992$��10.667(�$
 Hand��$��41���� -   $ labInfCropHoehe��$��0��$��Right=�$��	0,157,0,0q�$	��Top=����	#FF1DB992$��10.667(�$
 Hand��$��41���� -   $ labInfCropX��$��0��$��Right=�$��	0,178,0,0q�$	��Top=����	#FF1DB992$��10.667(�$
 Hand��$��41���� -   $ labInfCropY��$��0��$��Right=�$��	0,199,0,0q�$	��Top=����	#FF1DB992$��10.667(�$
 Hand��$��41���� -    $ sMaxFileSize��$
��Left=�$��
62,239,0,0q�$	��Top=�$	��299��$ 2Q�$ 1Q� �� TickPlacement$ TopLeft=��� -!   $ labInfMaxFileSize��$	��aus��$��Right=�$��	0,237,0,0q�$	��Top=����	#FF1DB992$��10.667(�$��41���� $��Größe��$
��Left=�$��
10,237,0,0q�$	��Top=����	#FFBABABA$��10.667(�$��47��$� �Aktiviere hier die Beschränkung für die Größe der Datei. ACHTUNG: Sobald die gewünschte Größe erreicht ist, wird das Video abgeschlossen.��$��Right=�9�  . $��Vorlagen�����	#FF1DB992.��� $��83��.+��� -"    ��Name$ 
comVorlage��$��
10,10,10,0q�$	��Top=� �� 
IsEditable . �� -#    ��Name$ btnVorlageDelete��$��Löschen��$
��Left=�$��	10,45,0,0q�$	��Top=�$	��191���� -$   $ btnVorlageSpeichern��$��	Speichern��$��	0,45,10,0q�$	��Top=�$��Right=�$	��191��� $��	0,0,10,10q�$��Right=�$	��448��$	��125��$��Bottom=�.+��� -%   $ 	btnExport��$��Exportieren��$��
10,0,10,10q�$��Bottom=��� -&   $ btnOpenFile��$��Eine andere Datei öffnen��$��
10,0,10,45q�$��Bottom=��� -'   $ btnCopyright��$��Rechtliches��$��
10,0,10,80q�$��Bottom=�� -(   $ gridTimeMagicInput��$y�	Collapsed=�.+�� 
���   �$	
 0.8Q�� $��320,142,319,157q�$��Center=�$��Center=�$	��120��$	��353��.+��� $*��$Den gewünschten Zeitpunkt eingeben:��$��
10,10,10,0q�$	��Top=�$ 	1.317,0.5&����	#FF1DB992�� -)     ��Name$  tbTimeMagicStunden��$
��Left=�$��23��$��	10,43,0,0q�! �� TextWrapping$
! Wrap=�$	��Top=�$��84��" �� SelectionBrush" �	#FF1DB992$ Hier Stunden, Yo!���� -*   $  tbTimeMagicMinuten��$
��Left=�$��23��$��	99,43,0,0q�$
! Wrap=�$	��Top=�$��84��" �	#FF1DB992$ Hier die Minuten!���� -+   $  tbTimeMagicSekunden��$��23��$��188,43,10,0q�$
! Wrap=�$	��Top=�" �	#FF1DB992$1 +Hier Sekunden inlc. Millisekunden. (00,000)���� -,   $ btnTimeMagicOk��$	��Oki��$��Right=�$��	0,0,10,10q�$��Bottom=�$��75���� --   $ btnTimeMagicCancel��$��Abbruch��$��Right=�$��	0,0,90,10q�$��Bottom=�$��75��� -.   $ gridSingleMagicInput��$y�	Collapsed=�.+�� 
���   �$	
 0.8Q�� $��307,146,307,127q�$��Center=�$��Center=�$	��146��$	��378��.+��� -/   $  labSingleMagicBeschreibung��$��Eingabe:��$��	10,0,8,84q�$��Bottom=����	#FF1DB992$��16(�$��33���� -0   $  tbSingleMagicInput��$��24��$��
10,0,10,49q�$
! Wrap=�$��Bottom=��� -1   $ btnSingleMagicOK��$	��Oki��$��Right=�$��	0,0,10,14q�$��Bottom=�$��75���� -2   $ btnSingleMagicCancel��$��Abbruch��$��Right=�$��	0,0,90,14q�$��Bottom=�$��75��� -3   $ gridLoading��$y�	Collapsed=�.+�� 
���   �$	
 0.8Q�� $��0,0,0,0q�$��Stretch=�$��Stretch=�.+��� -4   $ labLadeInfo��$	��Nix��$��Stretch=�$��10,71,0,223q�$��Center=����	#FF1DB992$	��125��$��Bottom=�� $��10,201,10,200q�$��Center=�$��18�����	#FF1DB992# � IsIndeterminate# . � -5   $ gridRechtliches��$y�	Collapsed=�.+�� 
���   �$	
 0.8Q�� $��263,61,225,57q�$��Center=�$��Center=�$	��301��$	��504��.+��� $"��Copyright ©            2016��$��	10,10,0,0q�$	��Top=����	#FF1DB992$
��Left=�$	��160���� -6   $ labInfCopyrightUser��$��Teto ●��$
��Left=�$��28��$��	87,10,0,0q�$	��Top=�$��48�����	#FFF5F7F6$
 Hand��$ 0.472,0.476&��� -7   $ btnCloseRechtliches��$��Okay, hab genug.��$��Right=�$��	0,0,10,10q�$��Bottom=�$	��135���� $��
06.01.2016��$��	0,10,10,0q�$	��Top=����	#FF1DB992$��Right=�$	��160��$��Right=��� $��10,43,10,45q�.���� .+��� $��WrapWithOverflow=���>_ Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Duis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zzril delenit augue duis dolore te feugait nulla facilisi. Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat. Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zzril delenit augue duis dolore te feugait nulla facilisi. Nam liber tempor cum soluta nobis eleifend option congue nihil imperdiet doming id quod mazim placerat facer possim assum. Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat. Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, At accusam aliquyam diam diam dolore dolores duo eirmod eos erat, et nonumy sed tempor et et invidunt justo labore Stet clita ea et gubergren, kasd magna no rebum. sanctus sea sed takimata ut vero voluptua. est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur�� �� =;Als ob es jemanden Interessiert, was zur Hölle hier steht.�� ��Da dieses Projekt ohne ffmpeg nicht funktioniert, hier ein Verweis auf deren Lizenz: https://www.ffmpeg.org/legal.html und http://www.gnu.org/licenses/old-licenses/lgpl-2.1.html�� ZXDas Verwendete Design: https://www.nuget.org/packages/Infragistics.Themes.MetroDark.Wpf/�� kiTrotzem Verweis auf die Nutzungslizenz: https://github.com/Graphicscore/WEBMcutter/blob/master/LICENSE.md�� =;Und auf GitHub: https://github.com/Graphicscore/WEBMcutter/�� XVVielen Dank an @Graphicscore für das Hosten des Repository auf seinem GitHub Account.�� ��Wie man den Source Dateien entnehmen kann, ist dies nun ein C# Project mit WPF als Fenster Framework. Es sieht toll aus aber wenn man Bilder in das WPF Image Object packen will, kann man sich gleich einen Platz in der Klapse bestellen.�� �� �� Changlog 2.0:�� - Es sieht fancy aus.�� - Es ist Open Source.�� fd- Es ist mehr GUI als vorher, um die möglichen Nachteile auszugleichen, gibt es neue Eingabemasken.�� ,*- Farbthematisch erinnert es jetzt an Pr0.�� US- Man kann das Video jetzt beschneiden (ist aber nicht so wirklich nice zu nutzen).�� ��- Man kann die maximale Ausgabegröße einstellen (Er bricht zwar einfach ab, sobald die entsprechende Größe erreicht wurde... Aber hey wenigstens geht es).�� YW- Noch Idiotensicherer - die Laufzeit kann nicht über das Video hinaus gezogen werden.�� - Kommt mit Macht im Keller.�� 