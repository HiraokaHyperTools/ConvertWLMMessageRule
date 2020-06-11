# Thunderbird メッセージフィルター

```
version="9"
logging="no"

name="A"
enabled="yes"
type="16"
action="Change priority"
actionValue="Highest"
condition="AND (from,contains,ABC)"
```

仕事タグをつける

```
action="AddTag"
actionValue="$label2"
```

## フィルターを実行するタイミング

`type`

1 = 新着メール受信時 迷惑メール分類前
16 = 手動で実行する
32 = 新着メール受信時 迷惑メール分類後
64 = メール送信後
128 = アーカイブ時
256 = 定期的 10 分ごと

## 条件

### 条件なし

```
condition="ALL ALL"
```

すべての条件

```
condition="AND (from,contains,ABC) AND (from,contains,DEF)"
```

いずれかの条件

```
condition="OR (from,contains,ABC) OR (from,contains,DEF)"
```

含む `contains`
含まない `doesn't contain`

差出人 from
件名 subject
本文 body
宛先 to
Cc cc
宛先または Cc to or cc
