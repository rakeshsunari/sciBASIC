﻿#Region "Microsoft.VisualBasic::28fd81d6ba14428fc2014e6d4c719427, Microsoft.VisualBasic.Core\Text\Parser\HtmlParser\HtmlStrips.vb"

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

    '     Module HtmlStrips
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: GetHtmlComments, GetInput, GetInputGroup, GetLinks, GetSelectInputGroup
    '                   GetSelectOptions, GetSelectValue, GetValue, HtmlLines, HtmlList
    '                   HTMLTitle, paragraph, Regexp, RemovesCSSstyles, RemovesFooter
    '                   RemovesHtmlComments, RemovesHtmlHead, RemovesHtmlStrong, RemovesImageLinks, RemovesJavaScript
    '                   RemoveTags, StripHTMLTags, stripTag, TrimResponseTail
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Text.RegularExpressions
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Serialization.JSON
Imports r = System.Text.RegularExpressions.Regex

Namespace Text.Parser.HtmlParser

    ''' <summary>
    ''' Html text document operations for a given html text
    ''' </summary>
    Public Module HtmlStrips

        ''' <summary>
        ''' 将<paramref name="html"/>文本之中的注释部分的字符串拿出来
        ''' </summary>
        ''' <param name="html"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        <Extension>
        Public Function GetHtmlComments(html As String) As String()
            Return r.Matches(html, "<![-]{2}.+?[-]{2}>", RegexICSng).ToArray
        End Function

        ''' <summary>
        ''' removes all of the html code comments from a given <paramref name="html"/> document.
        ''' </summary>
        ''' <param name="html"></param>
        ''' <returns></returns>
        <Extension>
        Public Function RemovesHtmlComments(html As StringBuilder) As StringBuilder
            For Each comment$ In html.ToString.GetHtmlComments
                Call html.Replace(comment, "")
            Next

            Return html
        End Function

        ''' <summary>
        ''' 从html文本之中解析出所有的链接
        ''' </summary>
        ''' <param name="html$"></param>
        ''' <returns></returns>
        <Extension> Public Function GetLinks(html$) As String()
            If String.IsNullOrEmpty(html) Then
                Return New String() {}
            Else
                Dim links$() = r _
                    .Matches(html, Regexp("a"), RegexICSng) _
                    .ToArray(AddressOf HtmlStrips.GetValue)
                Return links
            End If
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function Regexp(tagName As String) As String
            Return $"<{tagName}.+?</{tagName}>"
        End Function

        ''' <summary>
        ''' Parsing the title text from the html inputs.
        ''' </summary>
        ''' <param name="html"></param>
        ''' <returns></returns>
        <Extension> Public Function HTMLTitle(html As String) As String
            Dim title$ = r.Match(html, Regexp("title"), RegexICSng).Value

            If String.IsNullOrEmpty(title) Then
                title = "null"
            Else
                title = title.GetValue.TrimNewLine.Trim(" "c, ASCII.TAB)
            End If

            Return title
        End Function

        ''' <summary>
        ''' Removes the html tags from the text string.(这个函数会移除所有的html标签)
        ''' </summary>
        ''' <param name="s"></param>
        ''' <returns></returns>
        <ExportAPI("Html.Tag.Trim"), Extension> Public Function StripHTMLTags(s$, Optional stripBlank As Boolean = False) As String
            If String.IsNullOrEmpty(s) Then
                Return ""
            Else
                ' 在这里将<br/><br>标签替换为换行符
                ' 否则文本的排版可能会乱掉的
                s = r.Replace(s, "[<][/]?br[>]", vbLf, RegexICSng)
                s = r.Replace(s, "[<]h\d", vbLf & "<null", RegexICSng)
            End If

            ' 因为js和css都是夹在两个标签之间的，所以会被误认为是文本
            ' 在这里需要使用专门的函数来删除掉
            s = s.RemovesCSSstyles _
                 .RemovesJavaScript _
                 .RemovesFooter _
                 .RemovesHtmlHead

            s = r.Replace(s, "<[^>]+>", "")
            s = r.Replace(s, "</[^>]+>", "")

            If stripBlank Then
                s = s.StripBlank
                s = r.Replace(s, "(\n){3,}", vbLf & vbLf, RegexICSng)
            End If

            Return s
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="html"></param>
        ''' <param name="plainText">If this argument is value True, then all of the html format tag will be removes.</param>
        ''' <returns></returns>
        <Extension>
        Public Function paragraph(html$, Optional plainText As Boolean = False) As IEnumerable(Of String)
            Return html _
                .Matches(Regexp("p"), RegexICSng) _
                .Select(Function(p)
                            If plainText Then
                                Return p.StripHTMLTags
                            Else
                                Return p
                            End If
                        End Function) _
                .ToArray
        End Function

        <Extension>
        Public Function HtmlList(html As String) As String()
            Return html.Matches(Regexp("li"), RegexICSng).ToArray
        End Function

        ''' <summary>
        ''' 有些时候后面可能会存在多余的vbCrLf，则使用这个函数去除
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        <Extension> Public Function TrimResponseTail(value As String) As String
            If String.IsNullOrEmpty(value) Then
                Return ""
            End If

            Dim l% = Len(value)
            Dim i% = value.LastIndexOf(vbCrLf)

            If i = l - 2 Then
                Return Mid(value, 1, l - 2)
            Else
                Return value
            End If
        End Function

        ''' <summary>
        ''' 获取两个尖括号之间的内容
        ''' </summary>
        ''' <param name="html"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        <ExportAPI("Html.GetValue", Info:="Gets the string value between two wrapper character.")>
        <Extension> Public Function GetValue(html As String) As String
            Return html.GetStackValue(">", "<")
        End Function

        <Extension>
        Public Function GetInput(html$) As NamedValue(Of String)
            Dim input$ = r.Match(html, "<input.+?>", RegexICSng) _
                .Value _
                .Trim("<"c) _
                .StripHTMLTags(stripBlank:=True)
            Dim attrs = input.TagAttributes.ToArray
            Dim name$ = attrs.GetByKey("name", True).Value
            Dim value$ = attrs.GetByKey("value", True).Value
            Dim title$ = attrs.GetByKey("title", True).Value

            Return New NamedValue(Of String) With {
                .Name = name,
                .Value = value,
                .Description = title
            }
        End Function

        <Extension>
        Public Iterator Function GetInputGroup(html$) As IEnumerable(Of NamedValue(Of String))
            Dim inputs$() = r.Matches(html, "<input.+?>", RegexICSng).ToArray

            For Each input As String In inputs
                Yield input.GetInput
            Next
        End Function

        Public Function GetSelectOptions(html) As NamedCollection(Of String)
            Throw New NotImplementedException
        End Function

        Const selected$ = " " & NameOf(selected)

        Public Function GetSelectValue(html$) As NamedValue(Of String)
            Dim select$ = r.Match(html, "<select.+?/select", RegexICSng).Value
            Dim options$() = r.Matches([select], "<option.+?>", RegexICSng).ToArray

            [select] = r.Match([select], "<select.*?>", RegexICSng).Value

            Dim attrs = [select].TagAttributes.ToArray
            Dim name$ = attrs.GetByKey("name", True).Value
            Dim value$ = options _
                .Where(Function(s)
                           Return InStr(s, selected, CompareMethod.Text) > 0
                       End Function) _
                .FirstOrDefault _
               ?.Replace(selected, "") _
                .TagAttributes _
                .GetByKey("value", True) _
                .Value

            Return New NamedValue(Of String) With {
                .Name = name,
                .Value = value
            }
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        <Extension>
        Public Function GetSelectInputGroup(html$) As NamedValue(Of String)()
            Return r _
                .Matches(html, "<select.+?/select", RegexICSng) _
                .ToArray _
                .Select(AddressOf GetSelectValue) _
                .ToArray
        End Function

        ' <br><br/>
        ''' <summary>
        ''' The line break html tag in the html document. 
        ''' </summary>
        Const LineFeed$ = "(<br>)|(<br\s*/>)"

        ''' <summary>
        ''' Split the html text into lines by tags: ``&lt;br>`` or ``&lt;br/>``
        ''' </summary>
        ''' <param name="html$"></param>
        ''' <returns></returns>
        <Extension>
        Public Function HtmlLines(html$) As String()
            If html.StringEmpty Then
                Return {}
            Else
                Return r.Split(html, LineFeed, RegexICSng)
            End If
        End Function

        <Extension> Private Function stripTag(ByRef tag$) As String
            If tag Is Nothing Then
                tag = ""
            Else
                tag = tag _
                    .Trim("<"c) _
                    .Trim(">"c) _
                    .Trim("/"c)
            End If
            Return tag
        End Function

        ''' <summary>
        ''' 将<paramref name="html"/>中的``&lt;script>&lt;/script>``代码块删除
        ''' </summary>
        ''' <param name="html$"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        <Extension>
        Public Function RemovesJavaScript(html As String) As String
            ' <script>
            Return html.RemoveTags("script")
        End Function

        ''' <summary>
        ''' Removes all of the ``&lt;style>`` css styles block from a given <paramref name="html"/> document.
        ''' </summary>
        ''' <param name="html"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        <Extension>
        Public Function RemovesCSSstyles(html As String) As String
            ' <style>
            Return html.RemoveTags("style")
        End Function

        ''' <summary>
        ''' Removes all of the ``&lt;img>`` image links block from a given <paramref name="html"/> document.
        ''' </summary>
        ''' <param name="html"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        <Extension>
        Public Function RemovesImageLinks(html As String) As String
            ' <img>
            Return html.RemoveTags("img")
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        <Extension>
        Public Function RemovesHtmlHead(html As String) As String
            ' <head>
            Return html.RemoveTags("head")
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        <Extension>
        Public Function RemovesFooter(html As String) As String
            ' <footer>
            Return html.RemoveTags("footer")
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        <Extension>
        Public Function RemovesHtmlStrong(html As String) As String
            Dim buffer As New StringBuilder(html)

            For Each m As Match In r.Matches(html, "(<[/]?strong>)|(<[/]?b>)", RegexICSng)
                buffer.Replace(m.Value, "")
            Next

            Return buffer.ToString
        End Function

        Sub New()
            RegexpTimeout = 5
        End Sub

        <Extension>
        Public Function RemoveTags(html$, ParamArray tags$()) As String
            For Each tag As String In tags

                ' img 标签可能会在这里超时，如果没有<img></img>的话
                ' 则直接忽略掉这个错误
                Try
                    html = r.Replace(html, $"<{tag}.*?>.*?</{tag}>", "", RegexICSng)
                Catch ex As Exception When TypeOf ex Is TimeoutException
                    Call App.LogException(ex, tags.GetJson)
                Catch ex As Exception
                    Throw ex
                End Try

                html = r.Replace(html, $"<{tag}.*?>", "", RegexICSng)
            Next

            Return html
        End Function
    End Module
End Namespace
