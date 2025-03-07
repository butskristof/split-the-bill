# Split The Bill POC

## Requirements V1 
- The platform has members which register with their profile information
- Members can be part of a group 
- Expenses can be registered in a group
- Payments can be registered to a group
    - these are done by a group member to fund the expenses
- The amount owed is calculated per group member 
    - we tally up the total expense amount, divide it by the number of group members and subtract the amount the member already paid to the group

## Improvements V2
- Move away from one big group bill which is divided equally, but keep track of the "participants" per expense
    - these are set at the time of creation. If a member joins the group later, they should not be included in expenses for "all" members
- Record who paid for an expense
- For each relation between group members, calculate the balance between them 

# Improvements V3 
- Allow specifying partial attribution of an expense: equal splits, custom percentage or amount, ... 

## Sources
- https://github.com/ashishps1/awesome-low-level-design/blob/main/problems/splitwise.md 
