# Domain Models

## Menu

```csharp
class Menu{
    Menu Create();
    void AddDinner(Dinner dinner);
    void RemoveDinner(Dinner dinner);
    void UpdateSection(MenuSection section);
}
```

```json
"id": "0000",
"name": "Yummy Menu",
"description": "A menu with yummy food.",
"averageRating": 4.5,
"sections": [
    {
        "id": "0000",
        "name": "",
        "description": "",
        "items": [
            {
                "id": "0000",
                "name": "",
                "description": "",
                "price": 5.99
            }
        ]
    }
],
"CreatedDateTime": "",
"UpdatedDateTime": "",
"hostId": "0000",
"dinnerIds": ["0000","0000"],
"menuReviewIds": ["0000","0000"]
```
