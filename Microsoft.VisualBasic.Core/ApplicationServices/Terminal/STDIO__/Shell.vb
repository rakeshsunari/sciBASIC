﻿#Region "Microsoft.VisualBasic::0ea13648757d596ce2efe0336862a71f, Microsoft.VisualBasic.Core\ApplicationServices\Terminal\STDIO__\Shell.vb"

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

    '     Module Shell
    ' 
    '         Function: GetConsoleWindow, SetConsoleCtrlHandler, Shell, ShowWindow
    ' 
    '         Sub: HideConsoleWindow, ShowConsoleWindows
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Text.RegularExpressions
Imports System.Runtime.InteropServices

Namespace Terminal.STDIO__

    '
    ' * Created by SharpDevelop.
    ' * User: WORKGROUP
    ' * Date: 2015/2/26
    ' * Time: 0:13
    ' * 
    ' * To change this template use Tools | Options | Coding | Edit Standard Headers.
    ' 

    Public Module Shell

        ''' <summary>
        ''' You can create a console window In a Windows Forms project.  Project + properties, turn off "Enable application framework" 
        ''' And Set Startup Object To "Sub Main". 
        ''' 
        ''' Modify the Application.Run() statement To create the proper startup form, If necessary.
        ''' </summary>
        ''' <returns></returns>
        Public Declare Auto Function AllocConsole Lib "kernel32.dll" () As Boolean

        <DllImport("user32.dll")>
        Public Function ShowWindow(hWnd As IntPtr, nCmdShow As Integer) As Boolean
        End Function

        <DllImport("kernel32")>
        Public Function GetConsoleWindow() As IntPtr
        End Function

        <DllImport("Kernel32")>
        Private Function SetConsoleCtrlHandler(handler As EventHandler, add As Boolean) As Boolean
        End Function

        Private ReadOnly hConsole As IntPtr = GetConsoleWindow()

        Public Sub HideConsoleWindow()
            If IntPtr.Zero <> hConsole Then
                Call ShowWindow(hConsole, 0)
            End If
        End Sub

        ''' <summary>
        ''' 为WinForm应用程序分配一个终端窗口，这个函数一般是在Debug模式之下进行程序调试所使用的
        ''' </summary>
        Public Sub ShowConsoleWindows()
            If IntPtr.Zero <> hConsole Then
                Call ShowWindow(hConsole, 1)
            End If
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="CommandLine"></param>
        ''' <param name="WindowStyle"></param>
        ''' <param name="WaitForExit">If NOT, then the function returns the associated process id value. Else returns the process exit code.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Shell(CommandLine As String, Optional WindowStyle As System.Diagnostics.ProcessWindowStyle = ProcessWindowStyle.Normal, Optional WaitForExit As Boolean = False) As Integer
            Dim Tokens = Regex.Split(CommandLine, Global.Microsoft.VisualBasic.CommandLine.SPLIT_REGX_EXPRESSION)
            Dim EXE As String = Tokens.First
            Dim Arguments As String = Mid$(CommandLine, Len(EXE) + 1)
            Dim Process As New Process
            Dim pInfo As New ProcessStartInfo(EXE, Arguments)

            Process.StartInfo = pInfo
            Process.StartInfo.WindowStyle = WindowStyle

            Call Process.Start()

            If Not WaitForExit Then Return Process.Id

            Call Process.WaitForExit()
            Return Process.ExitCode
        End Function
    End Module
End Namespace
