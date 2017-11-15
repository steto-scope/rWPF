# rWPF

WPF UI Libary containing new Controls, Value Converters and Helpers. Binaries are available via NuGet:

    PM> Install-Package rWPF

## Usage

Declare in XAML the rwpf namespace and you're ready to go:

    <Window xmlns:r="http://rwpf.codeplex.com" ...>
      ...
      <StackPanel>
        <r:IPv4Field IP="{Binding IP}" />
        <r:IPv4Field IP="{Binding Mask}" NetmaskOnly="true" />
      </StackPanel>
      ...
    </Window>
    
## Documentation

See Wiki for a detailed description of features 
