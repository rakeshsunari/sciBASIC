﻿#Region "Microsoft.VisualBasic::c1e1205b2efd8723817832d34597a828, Data_science\DataMining\DataMining\ComponentModel\Evaluation\RegressionClassify.vb"

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

    '     Class RegressionClassify
    ' 
    '         Properties: actual, errors, predicts, sampleID
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace ComponentModel.Evaluation

    ''' <summary>
    ''' The regression classifier result.
    ''' </summary>
    Public Class RegressionClassify

        Public Property sampleID As String
        Public Property actual As Double
        Public Property predicts As Double

        Public ReadOnly Property errors As Double
            Get
                Return Math.Abs(predicts - actual)
            End Get
        End Property

    End Class
End Namespace
