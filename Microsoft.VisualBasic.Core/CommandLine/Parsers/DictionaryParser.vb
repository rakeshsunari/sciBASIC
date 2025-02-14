﻿#Region "Microsoft.VisualBasic::f332dc69aefd68863b03f3edc856ddbd, Microsoft.VisualBasic.Core\CommandLine\Parsers\DictionaryParser.vb"

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

    '     Class DictionaryParser
    ' 
    '         Function: TryParse
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Emit.Marshal
Imports Microsoft.VisualBasic.Text
Imports Microsoft.VisualBasic.Language

Namespace CommandLine.Parsers

    Public NotInheritable Class DictionaryParser

        ''' <summary>
        ''' 键值对之间使用分号分隔
        ''' </summary>
        ''' <param name="str$"></param>
        ''' <returns></returns>
        Public Shared Function TryParse(str$) As Dictionary(Of String, String)
            Dim chars As New Pointer(Of Char)(str$)
            Dim tmp As New List(Of Char)
            Dim out As New Dictionary(Of String, String)
            Dim t As New List(Of String)
            Dim markOpen As Boolean = False
            Dim left As Char

            Do While Not chars.EndRead
                Dim c As Char = +chars

                tmp += c

                If c = ASCII.Mark Then
                    If Not markOpen Then
                        If left = "="c Then
                            markOpen = True
                        End If
                    Else
                        If chars.Current = ";"c Then
                            markOpen = False
                            t += New String(tmp)
                            tmp.Clear()
                            chars += 1
                        End If
                    End If
                ElseIf c = ";"c Then
                    If Not markOpen Then
                        tmp.RemoveLast
                        t += New String(tmp)
                        tmp.Clear()
                    End If
                End If

                left = c
            Loop

            If tmp.Count > 0 Then
                t += New String(tmp)
            End If

            For Each var$ In t
                Dim value = var.GetTagValue("="c)
                out(value.Name) = value.Value.GetString("'")
            Next

            Return out
        End Function
    End Class
End Namespace
