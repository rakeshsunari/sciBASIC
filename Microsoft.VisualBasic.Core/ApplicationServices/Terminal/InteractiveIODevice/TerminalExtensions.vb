﻿#Region "Microsoft.VisualBasic::1dc754b47a61180b1bdf10c4c1438493, Microsoft.VisualBasic.Core\ApplicationServices\Terminal\InteractiveIODevice\TerminalExtensions.vb"

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

    '     Module TerminalEvents
    ' 
    ' 
    '         Delegate Sub
    ' 
    '             Properties: ConsoleHandleInvalid, CurrentSize
    ' 
    '             Sub: doEvents
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports System.Threading
Imports Microsoft.VisualBasic.Language

Namespace Terminal

    ''' <summary>
    ''' 这个终端事件会依赖于<see cref="App.Running"/>属性值来自动退出的
    ''' </summary>
    Public Module TerminalEvents

        Public Delegate Sub ResizeEventHandle(size As Size, oldSize As Size)

        Dim resizeHandles As New List(Of ResizeEventHandle)
        Dim oldSize As Size
        Dim eventThread As Thread

        Public ReadOnly Property CurrentSize As Size
            Get
                Return oldSize
            End Get
        End Property

        Private Sub doEvents()
            Do While App.Running
                If Console.WindowHeight <> oldSize.Height Then
                    RaiseEvent Resize()
                ElseIf Console.WindowWidth <> oldSize.Width Then
                    RaiseEvent Resize()
                End If

                Thread.Sleep(10)
            Loop
        End Sub

        Public ReadOnly Property ConsoleHandleInvalid As Boolean = False

        ''' <summary>
        ''' Terminal resize event for [<see cref="Console.WindowWidth"/>, <see cref="Console.WindowHeight"/>]
        ''' </summary>
        Public Custom Event Resize As ResizeEventHandle
            AddHandler(value As ResizeEventHandle)
                If resizeHandles.IndexOf(value) = -1 Then
                    resizeHandles += value
                End If

                If eventThread Is Nothing Then
                    Try
                        oldSize = New Size(Console.WindowWidth, Console.WindowHeight)
                        eventThread = New Thread(AddressOf doEvents)
                        eventThread.Start()
                    Catch ex As Exception  ' 可能是WindowsForm应用，则在这里就忽略掉这个错误了
                        Call App.LogException(ex)
                        _ConsoleHandleInvalid = True
                    End Try
                End If
            End AddHandler
            RemoveHandler(value As ResizeEventHandle)
                If resizeHandles.IndexOf(value) > -1 Then
                    resizeHandles.Remove(value)
                End If

                If resizeHandles.Count = 0 Then
                    eventThread.Abort()
                    eventThread = Nothing
                End If
            End RemoveHandler
            RaiseEvent()
                Dim [new] As New Size(Console.WindowWidth, Console.WindowHeight)

                For Each h As ResizeEventHandle In resizeHandles
                    Call h([new], oldSize)
                Next

                oldSize = [new]
            End RaiseEvent
        End Event
    End Module
End Namespace
