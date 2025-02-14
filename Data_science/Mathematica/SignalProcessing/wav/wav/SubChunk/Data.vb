﻿#Region "Microsoft.VisualBasic::31c7f97b66510e2e447684754acc60fc, Data_science\Mathematica\SignalProcessing\wav\wav\SubChunk\Data.vb"

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

    ' Class DataSubChunk
    ' 
    '     Properties: data
    ' 
    '     Function: loadData, ParseData
    ' 
    ' Structure Sample
    ' 
    '     Function: Parse16Bit, Parse8Bit, ToString
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports Microsoft.VisualBasic.Data.IO
Imports Microsoft.VisualBasic.Serialization.JSON

Public Class DataSubChunk : Inherits SubChunk

    Public Property data As Sample()

    Default Public ReadOnly Property channel(position As Integer) As Short()
        Get
            Return data _
                .Select(Function(sample) sample.channels(position)) _
                .ToArray
        End Get
    End Property

    Public Shared Function ParseData(wav As BinaryDataReader, format As FMTSubChunk) As DataSubChunk
        Do While wav.ReadString(4) <> "data"
            wav.Seek(-3, SeekOrigin.Current)
        Loop

        Call wav.Seek(-4, SeekOrigin.Current)

        Return New DataSubChunk With {
            .chunkID = wav.ReadString(4),
            .chunkSize = wav.ReadInt32,
            .data = DataSubChunk.loadData(wav, format).ToArray
        }
    End Function

    Private Shared Function loadData(wav As BinaryDataReader, format As FMTSubChunk) As IEnumerable(Of Sample)
        ' little endian
        wav.ByteOrder = ByteOrder.LittleEndian

        Select Case format.BitsPerSample
            Case 8
                Return Sample.Parse8Bit(wav, format.channels)
            Case 16
                Return Sample.Parse16Bit(wav, format.channels)
            Case Else
                Throw New NotImplementedException(format.GetJson)
        End Select
    End Function
End Class

Public Structure Sample

    ''' <summary>
    ''' 一个sample之中是由好几个声道的数据构成的
    ''' </summary>
    ''' <remarks>
    ''' 在这里使用Short来兼容8bit和16bit的数据
    ''' </remarks>
    Dim channels As Short()

    Public Overrides Function ToString() As String
        Return channels.GetJson
    End Function

    Public Shared Iterator Function Parse16Bit(wav As BinaryDataReader, channels As Integer) As IEnumerable(Of Sample)
        Dim sampleSize = channels * 2

        Do While Not wav.EndOfStream AndAlso (wav.Position + sampleSize <= wav.Length)
            Yield New Sample With {
                .channels = wav.ReadInt16s(channels)
            }
        Loop
    End Function

    Public Shared Iterator Function Parse8Bit(wav As BinaryDataReader, channels As Integer) As IEnumerable(Of Sample)
        Throw New NotImplementedException
    End Function

End Structure
