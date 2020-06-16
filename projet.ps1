gwmi win32_VideoController
gwmi win32_physicalmemory

Add-Type -AssemblyName PresentationFramework
[xml]$MainXAML = @"
<Window
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="MainWindow" Height="691" Width="1284">
<Grid>
        <Grid.Background>
            <LinearGradientBrush EndPoint="0.5,1" StartPoint="0.5,0">
                <GradientStop Color="Black" Offset="0"/>
                <GradientStop Color="#FF33BEFF"/>
                <GradientStop Color="#FF0E384B" Offset="1"/>
            </LinearGradientBrush>
        </Grid.Background>
        <ScrollViewer HorizontalAlignment="Left" Height="650" VerticalAlignment="Top" Width="333">
            <StackPanel HorizontalAlignment="Left" Height="720" VerticalAlignment="Top" Width="330">
                <Button x:Name="btnGeneral" Content="General" Height="80" Margin="0,0,15,0" FontFamily="Tw Cen MT Condensed Extra Bold" FontSize="48" Background="#FF33BEFF" Foreground="White"/>
                <Button x:Name="btnDisk" Content="Disk" Height="80" Margin="0,0,15,0" FontFamily="Tw Cen MT Condensed Extra Bold" FontSize="48" Background="#FF33BEFF" Foreground="White"/>
                <Button x:Name="btnRam" Content="RAM" Height="80" Margin="0,0,15,0" FontFamily="Tw Cen MT Condensed Extra Bold" FontSize="48" Background="#FF33BEFF" Foreground="White"/>
                <Button x:Name="btnCpu" Content="CPU" Height="80" Margin="0,0,15,0" FontFamily="Tw Cen MT Condensed Extra Bold" FontSize="48" Background="#FF33BEFF" Foreground="White"/>
                <Button x:Name="btnGraphicCard" Content="Graphic Card" Height="80" Margin="0,0,15,0" FontFamily="Tw Cen MT Condensed Extra Bold" FontSize="48" Background="#FF33BEFF" Foreground="White"/>
                <Button x:Name="btnUsers" Content="Users" Height="80" Margin="0,0,15,0" FontFamily="Tw Cen MT Condensed Extra Bold" FontSize="48" Background="#FF33BEFF" Foreground="White"/>
                <Button x:Name="btnService" Content="Services" Height="80" Margin="0,0,15,0" FontFamily="Tw Cen MT Condensed Extra Bold" FontSize="48" Background="#FF33BEFF" Foreground="White"/>
                <Button x:Name="btnStartup" Content="Startup" Height="80" Margin="0,0,15,0" FontFamily="Tw Cen MT Condensed Extra Bold" FontSize="48" Background="#FF33BEFF" Foreground="White"/>
                <Button x:Name="btnScheduledTasks" Content="Sheduled Tasks" Height="80" Margin="0,0,15,0" FontFamily="Tw Cen MT Condensed Extra Bold" FontSize="48" Background="#FF33BEFF" Foreground="White"/>
            </StackPanel>
        </ScrollViewer>
        
        <DockPanel x:Name="pnlInfo" HorizontalAlignment="Left" Height="633" LastChildFill="False" Margin="347,10,0,0" VerticalAlignment="Top" Width="909" Background="#FF8FD9FF"/>
    </Grid>
</Window>
"@

$MainReader = New-Object System.Xml.XmlNodeReader $MainXAML
$Window_Main= [Windows.Markup.XamlReader]::Load($MainReader)

$Window_Main.FindName("btnGeneral").Add_Click({
    $Window_Main.Close()
})
$Window_Main.FindName("btnDisk").Add_Click({
    $Window_Main.Close()
})
$Window_Main.FindName("btnRam").Add_Click({
    $Window_Main.Close()
})
$Window_Main.FindName("btnCpu").Add_Click({
    $Window_Main.Close()
})
$Window_Main.FindName("btnGraphicCard").Add_Click({
    $Window_Main.Close()
})
$Window_Main.FindName("btnUsers").Add_Click({
    $Window_Main.Close()
})
$Window_Main.FindName("btnService").Add_Click({
    $Window_Main.Close()
})
$Window_Main.FindName("btnStartup").Add_Click({
    $Window_Main.Close()
})
$Window_Main.FindName("btnScheduledTasks").Add_Click({
    $Window_Main.Close()
})

$Window_Main.ShowDialog()
