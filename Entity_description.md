# Entities

## Company
* Income
* Global stress level
* Contains break rooms

## Break rooms
* Slots (free/occupied)
* Can go over capacity?
  - Employees inside break rooms generate stress if over capacity?

## Employee
* Stress
  - Based on "Stress Influencers" (personal/global)
    * Different projects have different stress multipliers
	* Influenced by stress of nearby employees
  - Has a stress influence radius
* Cash
  - Money generation influenced by stress
* Movement
  - Can move to/from assigned break rooms
  - Is assigned a work station to return back to
  
## Employee states
* Working
* Walking
* Taking a break
* Ultra-stressed --> generates negative cash and has very high stress multiplier on nearby employees?