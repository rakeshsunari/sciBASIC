﻿#Region "Microsoft.VisualBasic::8cdfb3a798e95ad68f02410f067b810c, Microsoft.VisualBasic.Core\Net\HTTP\Stream\GZStream.vb"

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

    '     Module GZipStreamHandler
    ' 
    '         Function: AddGzipMagic, GZipAsBase64, GZipStream, UnGzipBase64, UnGzipStream
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports System.IO.Compression
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Linq

Namespace Net.Http

    ''' <summary>
    ''' 请注意，这个模块是处理http请求或者响应之中的gzip压缩的数据，
    ''' 对于zip压缩的数据需要使用<see cref="ZipStreamExtensions"/>模块之中的帮助函数来完成
    ''' </summary>
    Public Module GZipStreamHandler

        ''' <summary>
        ''' 如果得到的一个gzip压缩的数据块头部没有magic number的话，则使用这个方法手动的添加标记后再做解压缩
        ''' </summary>
        ''' <param name="data"></param>
        ''' <returns></returns>
        <Extension>
        Public Function AddGzipMagic(data As IEnumerable(Of Byte)) As IEnumerable(Of Byte)
            Return New Byte() {&H1F, &H8B}.JoinIterates(data)
        End Function

        ''' <summary>
        ''' Unzip the stream data in the <paramref name="base64"/> string.
        ''' </summary>
        ''' <param name="base64$"></param>
        ''' <returns></returns>
        ''' 
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        <Extension>
        Public Function UnGzipBase64(base64$) As MemoryStream
            Return Convert.FromBase64String(base64).UnGzipStream
        End Function

        ''' <summary>
        ''' 将输入的流数据进行gzip解压缩
        ''' </summary>
        ''' <remarks>
        ''' 使用这个函数得到的结果需要注意进行<see cref="IDisposable.Dispose()"/>,否则很容易造成内存泄漏
        ''' </remarks>
        ''' <param name="stream"></param>
        ''' <returns></returns>
        <Extension>
        Public Function UnGzipStream(stream As IEnumerable(Of Byte)) As MemoryStream
            Using gz As New Compression.GZipStream(New MemoryStream(stream.ToArray), CompressionMode.Decompress)
                Dim ms As New MemoryStream
                Call gz.CopyTo(ms)
                Return ms
            End Using
        End Function

        ''' <summary>
        ''' 对所输入的流进行gzip压缩
        ''' </summary>
        ''' <param name="stream"></param>
        ''' <returns></returns>
        <Extension>
        Public Function GZipStream(stream As Stream) As MemoryStream
            Dim ms As New MemoryStream()

            Using gz As New GZipStream(ms, CompressionMode.Compress)
                Call stream.Seek(Scan0, SeekOrigin.Begin)
                Call stream.CopyTo(gz)
            End Using

            ' we create the data array here once the GZIP stream has been disposed
            Dim data = ms.ToArray()
            ms.Dispose()
            ms = New MemoryStream(data)

            Return ms
        End Function

        ''' <summary>
        ''' 将目标流对象之中的数据进行zip压缩，然后转换为base64字符串
        ''' </summary>
        ''' <param name="stream"></param>
        ''' <returns></returns>
        <Extension> Public Function GZipAsBase64(stream As Stream) As String
            Dim bytes As Byte() = stream.GZipStream.ToArray
            Dim s$ = Convert.ToBase64String(bytes)
            Return s
        End Function
    End Module
End Namespace
