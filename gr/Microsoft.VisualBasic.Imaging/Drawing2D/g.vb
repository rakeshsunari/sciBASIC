﻿#Region "Microsoft.VisualBasic::a9a353e16399c616b39510459080acd1, gr\Microsoft.VisualBasic.Imaging\Drawing2D\g.vb"

' Author:
' 
'       asuka (amethyst.asuka@gcmodeller.org)
'       xie (genetics@smrucc.org)
'       xieguigang (xie.guigang@live.com)
' 
' Copyright (c) 2018 GPL3 Licensed
' 
' 
' GNU GENERAL PUBLIC LICENSE (GPL3)
' 
' 
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
' 
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.
' 
' You should have received a copy of the GNU General Public License
' along with this program. If not, see <http://www.gnu.org/licenses/>.



' /********************************************************************************/

' Summaries:

'     Delegate Sub
' 
' 
'     Module g
' 
'         Properties: ActiveDriver, DriverExtensionName
' 
'         Constructor: (+1 Overloads) Sub New
' 
'         Function: __getDriver, Allocate, CreateGraphics, (+2 Overloads) GraphicsPlots, (+2 Overloads) MeasureSize
'                   MeasureWidthOrHeight
' 
'         Sub: FillBackground
'         Class InternalCanvas
' 
'             Properties: bg, padding, size
' 
'             Function: InvokePlot
'             Operators: (+2 Overloads) +, <=, >=
' 
' 
' 
' 
' 
' /********************************************************************************/

#End Region

Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Text
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Driver
Imports Microsoft.VisualBasic.Imaging.PostScript
Imports Microsoft.VisualBasic.Imaging.SVG
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Language.Default
Imports Microsoft.VisualBasic.MIME.Markup.HTML.CSS
Imports Microsoft.VisualBasic.My.FrameworkInternal

Namespace Drawing2D

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="g">GDI+设备</param>
    ''' <param name="grct">绘图区域的大小</param>
    Public Delegate Sub IPlot(ByRef g As IGraphics, grct As GraphicsRegion)

    ''' <summary>
    ''' Data plots graphics engine common abstract. 
    ''' (在命令行中使用``graphic_driver=svg``来切换默认的图形引擎为SVG矢量图作图引擎)
    ''' </summary>
    ''' 
    <FrameworkConfig(GraphicDriverEnvironmentConfigName)>
    Public Module g

        ''' <summary>
        ''' 默认的页边距大小都是100个像素
        ''' </summary>
        Public Const DefaultPadding$ = "padding:100px 100px 100px 100px;"

        ''' <summary>
        ''' 与<see cref="DefaultPadding"/>相比而言，这个padding的值在坐标轴Axis的label的绘制上空间更加大
        ''' </summary>
        Public Const DefaultLargerPadding$ = "padding:100px 100px 150px 150px;"
        Public Const DefaultUltraLargePadding$ = "padding:150px 150px 300px 300px;"

        ''' <summary>
        ''' 所有的页边距都是零
        ''' </summary>
        Public Const ZeroPadding$ = "padding: 0px 0px 0px 0px;"
        Public Const MediumPadding$ = "padding: 45px 45px 45px 45px;"
        Public Const SmallPadding$ = "padding: 30px 30px 30px 30px;"
        Public Const TinyPadding$ = "padding: 5px 5px 5px 5px;"

        Friend Const GraphicDriverEnvironmentConfigName$ = "graphic_driver"

        ''' <summary>
        ''' 在这个模块的构造函数之中，程序会自动根据命令行所设置的环境参数来设置默认的图形引擎
        ''' 
        ''' ```
        ''' /@set graphic_driver=svg|gdi
        ''' ```
        ''' </summary>
        Sub New()
            Dim type$ = Strings.LCase(App.GetVariable(GraphicDriverEnvironmentConfigName))

            Select Case type
                Case "svg" : g.__defaultDriver = Drivers.SVG
                Case "gdi" : g.__defaultDriver = Drivers.GDI
                Case "ps" : g.__defaultDriver = Drivers.PS
                Case "wmf" : g.__defaultDriver = Drivers.WMF
                Case Else
                    g.__defaultDriver = Drivers.Default
            End Select

            Call $"The default graphics driver value is config as {g.__defaultDriver.Description}({type}).".__INFO_ECHO
        End Sub

        ''' <summary>
        ''' Get the result from commandline environment variable: ``graphic_driver``
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property ActiveDriver As Drivers
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return __defaultDriver
            End Get
        End Property

        ''' <summary>
        ''' Get the image file extension name for current default image driver.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property DriverExtensionName As String
            Get
                Select Case ActiveDriver
                    Case Drivers.SVG : Return "svg"
                    Case Drivers.GDI, Drivers.Default
                        Return "png"
                    Case Drivers.PS : Return "ps"
                    Case Drivers.WMF : Return "wmf"
                    Case Else
                        Throw New NotImplementedException(ActiveDriver.Description)
                End Select
            End Get
        End Property

        ''' <summary>
        ''' 用户所指定的图形引擎驱动程序类型，但是这个值会被开发人员设定的驱动程序类型的值所覆盖，
        ''' 通常情况下，默认引擎选用的是``gdi+``引擎
        ''' </summary>
        ReadOnly __defaultDriver As Drivers = Drivers.Default

        ''' <summary>
        ''' 这个函数不会返回<see cref="Drivers.Default"/>
        ''' </summary>
        ''' <param name="developerValue">程序开发人员所设计的驱动程序的值</param>
        ''' <returns></returns>
        Private Function __getDriver(developerValue As Drivers) As Drivers
            If developerValue <> Drivers.Default Then
                Return developerValue
            Else
                If g.__defaultDriver = Drivers.Default Then
                    ' 默认为使用gdi引擎
                    Return Drivers.GDI
                Else
                    Return g.__defaultDriver
                End If
            End If
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function MeasureWidthOrHeight(wh#, length%) As Single
            If wh > 0 AndAlso wh <= 1 Then
                Return wh * length
            Else
                Return wh
            End If
        End Function

        ReadOnly defaultSize As [Default](Of Size) = New Size(3600, 2000).AsDefault(Function(size) DirectCast(size, Size).IsEmpty)
        ReadOnly defaultPaddingValue As [Default](Of Padding) = CType(DefaultPadding, Padding).AsDefault(Function(pad) DirectCast(pad, Padding).IsEmpty)

        ''' <summary>
        ''' Data plots graphics engine. Default: <paramref name="size"/>:=(4300, 2000), <paramref name="padding"/>:=(100,100,100,100).
        ''' (用户可以通过命令行设置环境变量``graphic_driver``来切换图形引擎)
        ''' </summary>
        ''' <param name="size"></param>
        ''' <param name="padding">页边距</param>
        ''' <param name="bg">颜色值或者图片资源文件的url或者文件路径</param>
        ''' <param name="plotAPI"></param>
        ''' <param name="driver">驱动程序是默认与当前的环境参数设置相关的</param>
        ''' <returns></returns>
        ''' 
        <Extension>
        Public Function GraphicsPlots(ByRef size As Size,
                                      ByRef padding As Padding,
                                      bg$,
                                      plotAPI As IPlot,
                                      Optional driver As Drivers = Drivers.Default,
                                      Optional dpi$ = "100,100") As GraphicsData

            Dim driverUsed As Drivers = g.__getDriver(developerValue:=driver)

            size = size Or defaultSize
            padding = padding Or defaultPaddingValue

            Dim region As New GraphicsRegion With {
                .Size = size,
                .Padding = padding
            }

            Select Case driverUsed
                Case Drivers.SVG
                    Dim svg As New GraphicsSVG(size)

                    Call svg.Clear(bg.TranslateColor)
                    Call plotAPI(svg, region)

                    Return New SVGData(svg, size)
                Case Drivers.PS
                    Dim ps As New GraphicsPS(size)

                    Throw New NotImplementedException
                Case Drivers.WMF
                    Using wmf As New Wmf(size, WmfData.wmfTmp, bg)
                        Call plotAPI(wmf, region)
                        Return New WmfData(wmf.wmfFile, size)
                    End Using
                Case Else
                    ' using gdi+ graphics driver
                    ' 在这里使用透明色进行填充，防止当bg参数为透明参数的时候被CreateGDIDevice默认填充为白色
                    Using g As Graphics2D = size.CreateGDIDevice(Color.Transparent, dpi:=dpi)
                        Dim rect As New Rectangle(New Point, size)

                        With g.Graphics

                            Call .FillBackground(bg$, rect)

                            .CompositingQuality = CompositingQuality.HighQuality
                            .CompositingMode = CompositingMode.SourceOver
                            .InterpolationMode = InterpolationMode.HighQualityBicubic
                            .PixelOffsetMode = PixelOffsetMode.HighQuality
                            .SmoothingMode = SmoothingMode.HighQuality
                            .TextRenderingHint = TextRenderingHint.ClearTypeGridFit

                        End With

                        Call plotAPI(g, region)

                        Return New ImageData(g.ImageResource, size)
                    End Using
            End Select
        End Function

        ''' <summary>
        ''' 自动根据表达式的类型来进行纯色绘制或者图形纹理画刷绘制
        ''' </summary>
        ''' <param name="g"></param>
        ''' <param name="bg$">
        ''' 1. 可能为颜色表达式
        ''' 2. 可能为图片的路径
        ''' 3. 可能为base64图片字符串
        ''' </param>
        <Extension>
        Public Sub FillBackground(ByRef g As Graphics, bg$, rect As Rectangle)
            Dim bgColor As Color = bg.TranslateColor(throwEx:=False)

            If Not bgColor.IsEmpty Then
                Call g.FillRectangle(New SolidBrush(bgColor), rect)
            Else
                ' If the bg is not a file, then will try decode it as base64 string image. 
                Dim res As Image = bg.LoadImage(base64:=Not bg.FileExists)
                Call g.DrawImage(res, rect)
            End If
        End Sub

        ''' <summary>
        ''' <see cref="Graphics.MeasureString(String, Font)"/> extensions
        ''' </summary>
        ''' <param name="text$"></param>
        ''' <param name="g"></param>
        ''' <param name="font"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        <Extension>
        Public Function MeasureSize(text$, g As IGraphics, font As Font) As SizeF
            Return g.MeasureString(text, font)
        End Function

        ''' <summary>
        ''' <see cref="Graphics.MeasureString(String, Font)"/> extensions
        ''' </summary>
        ''' <param name="text$"></param>
        ''' <param name="g"></param>
        ''' <param name="font"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        <Extension>
        Public Function MeasureSize(text$, g As IGraphics, font As Font, scale As (x#, y#)) As SizeF
            Dim size As SizeF

            g.ScaleTransform(scale.x, scale.y)
            size = g.MeasureString(text, font)
            g.ScaleTransform(1, 1)

            Return size
        End Function

        ''' <summary>
        ''' Data plots graphics engine.
        ''' </summary>
        ''' <param name="size"></param>
        ''' <param name="bg"></param>
        ''' <param name="plot"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        <Extension>
        Public Function GraphicsPlots(plot As Action(Of IGraphics), ByRef size As Size, ByRef padding As Padding, bg$) As GraphicsData
            Return GraphicsPlots(size, padding, bg, Sub(ByRef g, rect) Call plot(g))
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function Allocate(Optional size As Size = Nothing, Optional padding$ = DefaultPadding, Optional bg$ = "white") As InternalCanvas
            Return New InternalCanvas With {
                .size = size,
                .bg = bg,
                .padding = padding
            }
        End Function

        <Extension>
        Public Function CreateGraphics(img As GraphicsData) As IGraphics
            If img.Driver = Drivers.SVG Then
                Dim svg = DirectCast(img, SVGData).SVG
                Return New GraphicsSVG(svg)
            Else
                Return Graphics2D.Open(DirectCast(img, ImageData).Image)
            End If
        End Function

        ''' <summary>
        ''' 可以借助这个画布对象创建多图层的绘图操作
        ''' </summary>
        Public Class InternalCanvas

            Dim plots As New List(Of IPlot)

            Public Property size As Size
            Public Property padding As Padding
            Public Property bg As String

            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Public Function InvokePlot() As GraphicsData
                Return GraphicsPlots(
                    size, padding, bg,
                    Sub(ByRef g, rect)

                        For Each plot As IPlot In plots
                            Call plot(g, rect)
                        Next
                    End Sub)
            End Function

            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Public Shared Operator +(g As InternalCanvas, plot As IPlot) As InternalCanvas
                g.plots += plot
                Return g
            End Operator

            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Public Shared Operator +(g As InternalCanvas, plot As IPlot()) As InternalCanvas
                g.plots += plot
                Return g
            End Operator

            Public Shared Narrowing Operator CType(g As InternalCanvas) As GraphicsData
                Return g.InvokePlot
            End Operator

            ''' <summary>
            ''' canvas invoke this plot.
            ''' </summary>
            ''' <param name="g"></param>
            ''' <param name="plot"></param>
            ''' <returns></returns>
            Public Shared Operator <=(g As InternalCanvas, plot As IPlot) As GraphicsData
                Dim size As Size = g.size
                Dim margin = g.padding
                Dim bg As String = g.bg

                Return GraphicsPlots(size, margin, bg, plot)
            End Operator

            Public Shared Operator >=(g As InternalCanvas, plot As IPlot) As GraphicsData
                Throw New NotSupportedException
            End Operator
        End Class
    End Module
End Namespace
