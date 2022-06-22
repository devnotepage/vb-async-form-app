Public Class FormMain

    ' 開始ボタンクリック
    Private Sub ButtonStart_Click(sender As Object, e As EventArgs) Handles ButtonStart.Click

        ' UI操作禁止
        Me.Enabled = False

        Me.LabelStatus.Text = "Start!"

        ' 非同期実行(別スレッド)
        Dim task As Task = Task.Run(
            Sub()

                ' 重い処理
                ' UIを制御するためフォームのインスタンスを渡しておく
                HeavyProcess(Me)

            End Sub
        )

    End Sub

    ' 重い処理
    Public Sub HeavyProcess(form As FormMain)

        ' 重い処理
        Dim max As Integer = 999999999
        Dim percent_prev As Integer = -1
        For i = 0 To max

            ' パーセント算出(切り捨て)
            Dim percent As Integer = Math.Truncate(i / max * 100)

            ' メッセージ更新(パーセント変化時のみ)
            If percent <> percent_prev Then
                Dim message As String = String.Format("{0}% ({1}/{2})", percent, i, max)
                form.Control(ControlID.UpdateProgress, message)
                percent_prev = percent
            End If

        Next

        ' 処理完了
        form.Control(ControlID.Complete, Nothing)

    End Sub

    ' UI制御ID
    Public Enum ControlID
        UpdateProgress
        Complete
    End Enum

    ' UI制御処理
    Public Sub Control(id As ControlID, message As String)

        ' Invoke必要チェック
        If Me.InvokeRequired Then
            ' Invoke必要
            Me.Invoke(New DelegateControlMain(AddressOf ControlMain), id, message)
        Else
            ' Invoke不要
            ControlMain(id, message)
        End If

    End Sub

    ' UI制御処理メイン
    Delegate Sub DelegateControlMain(id As ControlID, message As String)
    Public Sub ControlMain(id As ControlID, message As String)

        Select Case id
            Case ControlID.UpdateProgress
                ' メッセージ更新
                Me.LabelProgress.Text = message
            Case ControlID.Complete
                ' UI操作許可
                Me.LabelStatus.Text = "Complete!"
                Me.Enabled = True
            Case Else
                ' UI操作許可
                Me.LabelStatus.Text = "Error!"
                Me.Enabled = True
        End Select

    End Sub

End Class
