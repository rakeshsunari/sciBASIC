﻿#Region "Microsoft.VisualBasic::2c82045a56ccf9a6611a6f19d9df9155, Microsoft.VisualBasic.Core\ApplicationServices\Terminal\PipelineCLI.vb"

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

    '     Module PipelineCLI
    ' 
    '         Sub: Start
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports System.Runtime.CompilerServices

Namespace Terminal

    ''' <summary>
    ''' a | b - 管道命令在读写方面更加适合于文本数据，由于省去了IO的时间，故而效率较高
    ''' </summary>
    Public Module PipelineCLI

        ''' <summary>
        ''' 使用管道的方法启动下游的应用程序
        ''' </summary>
        ''' <param name="app"></param>
        ''' <param name="args"></param>
        ''' <remarks>
        ''' http://stackoverflow.com/questions/30546522/how-to-use-a-pipe-between-two-processes-in-process-start
        ''' 
        ''' let the OS do it. ``StartInfo.FileName = "cmd"`` then prepend ``executablepath`` to params so it looks 
        ''' the way you would enter it in a command window; 
        ''' ``StartInfo.Arguments = params`` then start the process 
        ''' 
        ''' – Plutonix May 30 '15 at 15:13
        ''' </remarks>
        ''' 
        <Extension>
        Public Sub Start(buf As Stream, app As String, Optional args As String = "")
            Dim Proc As New Process

            Proc.StartInfo.CreateNoWindow = True
            Proc.StartInfo.UseShellExecute = False
            Proc.StartInfo.FileName = app
            Proc.StartInfo.RedirectStandardOutput = True
            Proc.StartInfo.RedirectStandardError = True
            Proc.StartInfo.RedirectStandardInput = True

            Proc.Start()

            Using writer As New BinaryWriter(Proc.StandardInput.BaseStream)
                Dim read As New BinaryReader(buf)

                Do While True
                    Dim bufs As Byte() = read.ReadBytes(1024)
                    Call writer.Write(bufs, Scan0, bufs.Length)
                Loop

                Call writer.Flush()
                Call writer.Close()
            End Using

            Call Proc.WaitForExit()
        End Sub
    End Module
End Namespace
