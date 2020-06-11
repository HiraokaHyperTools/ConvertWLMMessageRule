# Windows Live Mail メッセージ ルール

## Logic

- 0 = final
- 1 = thisOrNext
- 2 = thisAndNext

## Flags

- 2 = andValues, 0 = orValues
- 1 = notContains, 0 = contains

## Type

- 8 = 件名
- 9 = 本文
- 10 = 宛先
- 11 = CC
- 12 = 差出人
- 13 = 重要度 -> Typ19 2=高 1=低
- 14 = 添付ファイルあり -> NoValue
- 15 = 指定したサイズ以上 -> Typ19 KB 単位
- 19 = 指定したアカウントへ送信 -> Typ31 "account{8A5968F1-E503-4D9F-B5AD-9152275B4FF8}.oeaccount"
- 20 = すべてのメッセージに適用
- 22 = 宛先 or CC
- 31 = 指定したセキュリティ -> Typ19 1=署名つき 2=暗号化された

## ValueType

- 19 = DWORD
- 31 = UnicodeString (generic text)
- 65 = UnicodeString
