﻿#Region "Microsoft.VisualBasic::581557608c03c422167a0717138431b7, Microsoft.VisualBasic.Core\Extensions\StringHelpers\Parser.vb"

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

    ' Module PrimitiveParser
    ' 
    '     Function: Eval, IsBooleanFactor, IsInteger, IsNumeric, (+2 Overloads) ParseBoolean
    '               ParseDate, ParseDouble, ParseInteger, ParseLong, ParseSingle
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Scripting.Runtime
Imports Microsoft.VisualBasic.Text

''' <summary>
''' Simple type parser extension function for <see cref="String"/>
''' </summary>
Public Module PrimitiveParser

    ''' <summary>
    ''' Evaluate the given string expression as numeric value 
    ''' </summary>
    ''' <param name="expression$"></param>
    ''' <param name="default#"></param>
    ''' <returns></returns>
    Public Function Eval(expression$, default#) As Double
        If expression Is Nothing Then
            Return [default]
        Else
            Return Conversion.Val(expression)
        End If
    End Function

    ''' <summary>
    ''' 用于匹配任意实数的正则表达式
    ''' 
    ''' (这个正则表达式有一个bug，会匹配上一个单独的字母E)
    ''' </summary>
    ''' <remarks>
    ''' 这个表达式并不用于<see cref="IsNumeric"/>, 但是其他的模块的代码可能会需要这个通用的表达式来做一些判断
    ''' </remarks>
    Public Const NumericPattern$ = "[-]?\d*(\.\d+)?([eE][-]?\d*)?"

#Region "text token pattern assert"
    ' 2019-04-17 正则表达式的执行效率过低

    ''' <summary>
    ''' Is this token value string is a number?
    ''' </summary>
    ''' <returns></returns>
    <ExportAPI("IsNumeric", Info:="Is this token value string is a number?")>
    <Extension> Public Function IsNumeric(num As String) As Boolean
        Dim dotCheck As Boolean = False
        Dim c As Char = num(Scan0)
        Dim offset As Integer = 0

        ' 修复正则匹配的bug
        If num = "e" OrElse num = "E" Then
            Return False
        End If

        If c = "-"c OrElse c = "+"c Then
            ' check for number sign symbol
            '
            ' +3.0
            ' -3.0
            offset = 1
        ElseIf c = "."c Then
            ' check for 
            ' 
            ' .1 (0.1)
            offset = 1
            dotCheck = True
        End If

        For i As Integer = offset To num.Length - 1
            c = num(i)

            If Not c Like numbers Then
                If c = "."c Then
                    If dotCheck Then
                        Return False
                    Else
                        dotCheck = True
                    End If
                ElseIf c = "E"c OrElse c = "e"c Then
                    Return IsInteger(num, i + 1)
                Else
                    Return False
                End If
            End If
        Next

        Return True
    End Function

    ReadOnly numbers As Index(Of Char) = {"0"c, "1"c, "2"c, "3"c, "4"c, "5"c, "6"c, "7"c, "8"c, "9"c}

    Public Function IsInteger(num As String, Optional offset As Integer = 0) As Boolean
        Dim c As Char = num(Scan0)

        ' check for number sign symbol
        If c = "-"c OrElse c = "+"c Then
            offset += 1
        End If

        For i As Integer = offset To num.Length - 1
            c = num(i)

            If Not c Like numbers Then
                Return False
            End If
        Next

        Return True
    End Function
#End Region

    ''' <summary>
    ''' <see cref="Integer"/> text parser
    ''' </summary>
    ''' <param name="s"></param>
    ''' <returns></returns>
    ''' 
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    <Extension>
    Public Function ParseInteger(s As String) As Integer
        Return CInt(Val(Trim(s)))
    End Function

    ''' <summary>
    ''' <see cref="Long"/> text parser
    ''' </summary>
    ''' <param name="s"></param>
    ''' <returns></returns>
    ''' 
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    <Extension>
    Public Function ParseLong(s As String) As Long
        Return CLng(Val(Trim(s)))
    End Function

    ''' <summary>
    ''' <see cref="Double"/> text parser. (这个是一个非常安全的字符串解析函数)
    ''' </summary>
    ''' <param name="s"></param>
    ''' <returns></returns>
    ''' 
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    <Extension>
    Public Function ParseDouble(s As String) As Double
        If s Is Nothing Then
            Return 0
        Else
            Return ParseNumeric(s)
        End If
    End Function

    ''' <summary>
    ''' <see cref="Single"/> text parser
    ''' </summary>
    ''' <param name="s"></param>
    ''' <returns></returns>
    ''' 
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    <Extension>
    Public Function ParseSingle(s As String) As Single
        Return CSng(Val(Trim(s)))
    End Function

    ''' <summary>
    ''' <see cref="Date"/> text parser
    ''' </summary>
    ''' <param name="s"></param>
    ''' <returns></returns>
    ''' 
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    <Extension>
    Public Function ParseDate(s As String) As Date
        Return Date.Parse(Trim(s))
    End Function

    ''' <summary>
    ''' Convert the string value into the boolean value, this is useful to the text format configuration file into data model.
    ''' </summary>
    ReadOnly booleans As New SortedDictionary(Of String, Boolean) From {
 _
        {"t", True}, {"true", True},
        {"1", True},
        {"y", True}, {"yes", True}, {"ok", True},
        {"ok!", True},
        {"success", True}, {"successful", True}, {"successfully", True}, {"succeeded", True},
        {"right", True},
        {"wrong", False},
        {"failure", False}, {"failures", False},
        {"exception", False},
        {"error", False}, {"err", False},
        {"f", False}, {"false", False},
        {"0", False},
        {"n", False}, {"no", False}
    }

    ''' <summary>
    ''' 目标字符串是否可以被解析为一个逻辑值
    ''' </summary>
    ''' <param name="token"></param>
    ''' <returns></returns>
    Public Function IsBooleanFactor(token As String) As Boolean
        If String.IsNullOrEmpty(token) Then
            Return False
        Else
            Return booleans.ContainsKey(token.ToLower)
        End If
    End Function

    ''' <summary>
    ''' Convert the string value into the boolean value, this is useful to the text format configuration file into data model.
    ''' (请注意，空值字符串为False，如果字符串不存在与单词表之中，则也是False)
    ''' </summary>
    ''' <param name="str"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <ExportAPI("ParseBoolean")>
    <Extension>
    Public Function ParseBoolean(str$) As Boolean
        If String.IsNullOrEmpty(str) Then
            Return False
        Else
            str = str.ToLower.Trim
        End If

        If booleans.ContainsKey(key:=str) Then
            Return booleans(str)
        Else
#If DEBUG Then
            Call $"""{str}"" {NameOf([Boolean])} (null_value_definition)  ==> False".__DEBUG_ECHO
#End If
            Return False
        End If
    End Function

    <ExportAPI("ParseBoolean")>
    <Extension>
    Public Function ParseBoolean(ch As Char) As Boolean
        If ch = ASCII.NUL Then
            Return False
        End If

        Select Case ch
            Case "y"c, "Y"c, "t"c, "T"c, "1"c
                Return True
            Case "n"c, "N"c, "f"c, "F"c, "0"c
                Return False
        End Select

        Return True
    End Function
End Module
