﻿#Region "Microsoft.VisualBasic::ac611a33a6216095f867c9f50576c0a3, Data_science\Mathematica\SignalProcessing\wav\wav\SubChunk\FMT.vb"

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

    ' Enum wFormatTag
    ' 
    ' 
    '  
    ' 
    ' 
    ' 
    ' Enum Channels
    ' 
    ' 
    '  
    ' 
    ' 
    ' 
    ' Class FMTSubChunk
    ' 
    '     Properties: audioFormat, BitsPerSample, BlockAlign, ByteRate, channels
    '                 isPCM, SampleRate
    ' 
    '     Function: ParseChunk
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Data.IO

Public Enum wFormatTag
    ''' <summary>
    ''' PCM
    ''' </summary>
    WAVE_FORMAT_PCM = &H1
    ''' <summary>
    ''' IEEE float
    ''' </summary>
    WAVE_FORMAT_IEEE_FLOAT = &H3
    ''' <summary>
    ''' 8-bit ITU-T G.711 A-law
    ''' </summary>
    WAVE_FORMAT_ALAW = &H6
    ''' <summary>
    ''' 8-bit ITU-T G.711 µ-law
    ''' </summary>
    WAVE_FORMAT_MULAW = &H7
    ''' <summary>
    ''' Determined by SubFormat
    ''' </summary>
    WAVE_FORMAT_EXTENSIBLE = &HFFFE
End Enum

Public Enum Channels
    Mono = 1
    Stereo = 2
End Enum

''' <summary>
''' The "fmt " subchunk describes the sound data's format
''' </summary>
Public Class FMTSubChunk : Inherits SubChunk

    ''' <summary>
    ''' PCM = 1 (i.e. Linear quantization) Values other than 1 indicate some form of compression.
    ''' </summary>
    ''' <returns></returns>
    Public Property audioFormat As wFormatTag
    Public Property channels As Channels
    ''' <summary>
    ''' 22.05KHz/44.1kHz/48KHz
    ''' </summary>
    ''' <returns></returns>
    Public Property SampleRate As Integer
    ''' <summary>
    ''' (bytes_per sec) 音频的码率，每秒播放的字节数。
    ''' 
    ''' ```
    ''' SampleRate * NumChannels * BitsPerSample / 8
    ''' ```
    ''' </summary>
    ''' <returns></returns>
    Public Property ByteRate As Integer
    ''' <summary>
    ''' ```
    ''' NumChannels * BitsPerSample / 8
    ''' ```
    ''' 
    ''' The number Of bytes For one sample including
    ''' all channels. I wonder what happens When
    ''' this number isn't an integer?
    ''' </summary>
    ''' <returns></returns>
    Public Property BlockAlign As Integer
    ''' <summary>
    ''' 8 bits = 8, 16 bits = 16, etc.
    ''' </summary>
    ''' <returns></returns>
    Public Property BitsPerSample As Integer

    ''' <summary>
    ''' Pulse Code Modulation
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property isPCM As Boolean
        Get
            Return audioFormat = 1
        End Get
    End Property

    Public Shared Function ParseChunk(wav As BinaryDataReader) As FMTSubChunk
        Dim subchunk1ID As String = wav.ReadString(4)

        ' number data is in little endian
        wav.ByteOrder = ByteOrder.LittleEndian

        Return New FMTSubChunk With {
            .chunkID = subchunk1ID,
            .chunkSize = wav.ReadInt32,
            .audioFormat = wav.ReadInt16,
            .channels = wav.ReadInt16,
            .SampleRate = wav.ReadInt32,
            .ByteRate = wav.ReadInt32,
            .BlockAlign = wav.ReadInt16,
            .BitsPerSample = wav.ReadInt16
        }
    End Function
End Class
