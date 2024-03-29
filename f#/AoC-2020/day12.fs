module day12

open System.Drawing


type Direction =
    | East = 1
    | South = 2
    | West = 3
    | North = 4

let intToDirection directionInt =
    match directionInt * (if directionInt < 0 then -1 else 1) with
    | 1 -> Direction.East
    | 2 -> Direction.South
    | 3 -> Direction.West
    | 4 -> Direction.North
    | _ -> failwith $"Invalid integer ({directionInt}) for Direction"

let directionToMotionPoint (direction: Direction) (distance: int) : Point =
    match direction with
    | Direction.East -> Point(1 * distance, 0)
    | Direction.South -> Point(0, -1 * distance)
    | Direction.West -> Point(-1 * distance, 0)
    | Direction.North -> Point(0, 1 * distance)
    | _ -> failwith $"Invalid direction {direction}"

type Ship =
    { mutable direction: Direction
      mutable position: int * int
      mutable waypoint: Point }

type Ship with

    member this.turnShip (left: bool) (degrees: int) =
        let currentDirection = int this.direction
        let mutable turnUnits: int = degrees / 90
        if left then
            turnUnits <- 4 - turnUnits
        let mutable newDirection = currentDirection + turnUnits
        if newDirection > 4 then
            newDirection <- newDirection % 4
        this.direction <- intToDirection newDirection

    member this.moveForward(distance: int) = this.moveShip this.direction distance

    member this.moveShip (direction: Direction) (distance: int) =
        let mx, my =
            match direction with
            | Direction.East -> 1 * distance, 0
            | Direction.South -> 0, -1 * distance
            | Direction.West -> -1 * distance, 0
            | Direction.North -> 0, 1 * distance
            | _ -> failwith "Invalid Direction"

        let x, y = this.position
        this.position <- x + mx, y + my
        
    member this.moveWaypoint (direction: Direction) (distance: int) =
        let motion = directionToMotionPoint direction distance
        this.waypoint <- Point(this.waypoint.X + motion.X, this.waypoint.Y + motion.Y)
        
    member this.rotateWaypoint (left: bool) (degrees: float) =
        let actualDegrees = if left then degrees else -degrees
        let radians = System.Math.PI * actualDegrees / 180.0
        let x = (float this.waypoint.X) * System.Math.Cos(radians) - (float this.waypoint.Y) * System.Math.Sin(radians)
        let y = (float this.waypoint.X) * System.Math.Sin(radians) + (float this.waypoint.Y) * System.Math.Cos(radians)
        this.waypoint <- Point(int (System.Math.Round(x)), int (System.Math.Round(y)))
        
    member this.moveToWaypoint (speed: int) =
        let target = Point(this.waypoint.X * speed, this.waypoint.Y * speed)
        let cx, cy = this.position
        this.position <- cx + target.X, cy + target.Y        

    member this.manhattanDistance: int =
        let x, y = this.position
        abs x + abs y

let day12part1 (lines: string array) : string =
    let ship: Ship =
        { direction = Direction.East
          position = 0, 0
          waypoint = Point(0, 0) }

    for line in lines do
        let value = line.Substring(1) |> int

        match line.Substring(0, 1) with
        | "N" -> ship.moveShip Direction.North value
        | "E" -> ship.moveShip Direction.East value
        | "S" -> ship.moveShip Direction.South value
        | "W" -> ship.moveShip Direction.West value
        | "F" -> ship.moveForward value
        | "L" -> ship.turnShip true value
        | "R" -> ship.turnShip false value
        | _ -> failwith $"Unrecognised command. {line}"

    ship.manhattanDistance |> string

let day12part2 (lines: string array) : string =
    let ship: Ship = {
        direction = Direction.East
        position = 0, 0
        waypoint = Point(10, 1)
    }
    
    for line in lines do
        let value = line.Substring(1) |> int

        match line.Substring(0, 1) with
        | "N" -> ship.moveWaypoint Direction.North value
        | "E" -> ship.moveWaypoint Direction.East value
        | "S" -> ship.moveWaypoint Direction.South value
        | "W" -> ship.moveWaypoint Direction.West value
        | "F" -> ship.moveToWaypoint value
        | "L" -> ship.rotateWaypoint true (float value)
        | "R" -> ship.rotateWaypoint false (float value)
        | _ -> failwith $"Unrecognised command. {line}"

    ship.manhattanDistance |> string