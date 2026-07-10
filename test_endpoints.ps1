$ErrorActionPreference = "Stop"
$gateway_url = "http://localhost:8080"
$token = ""
$hotel_id = ""
$room_id = ""
$booking_id = ""

Write-Host "1. Login (admin)"
$loginBody = @{ username = "admin"; password = "admin123" } | ConvertTo-Json
$loginResponse = Invoke-RestMethod -Uri "$gateway_url/api/auth/login" -Method Post -ContentType "application/json" -Body $loginBody
$token = $loginResponse.token
Write-Host "Token received: $($token.Substring(0, 10))..."

$headers = @{ Authorization = "Bearer $token" }
$jsonContentType = "application/json"

Write-Host "`n2. Crear hotel"
$hotelBody = @{ name = "Hotel Royal Bogotá"; city = "Bogotá"; address = "Calle 100 #15-20"; description = "Hotel de lujo" } | ConvertTo-Json
$createHotelResponse = Invoke-RestMethod -Uri "$gateway_url/api/hotels" -Method Post -Headers $headers -ContentType $jsonContentType -Body $hotelBody
$hotel_id = $createHotelResponse.id
if (-not $hotel_id) { $hotel_id = $createHotelResponse.hotelId } # Based on postman script
Write-Host "Hotel created with ID: $hotel_id"

Write-Host "`n3. Actualizar hotel"
$updateHotelBody = @{ name = "Hotel Royal Bogotá UPDATED"; city = "Bogotá"; address = "Calle 100 #15-20"; description = "Renovado" } | ConvertTo-Json
Invoke-RestMethod -Uri "$gateway_url/api/hotels/$hotel_id" -Method Put -Headers $headers -ContentType $jsonContentType -Body $updateHotelBody
Write-Host "Hotel updated."

Write-Host "`n4. Listar hoteles"
$hotels = Invoke-RestMethod -Uri "$gateway_url/api/hotels" -Method Get
Write-Host "Total hotels: $($hotels.Count)"

Write-Host "`n5. Detalle de hotel"
$hotel = Invoke-RestMethod -Uri "$gateway_url/api/hotels/$hotel_id" -Method Get
Write-Host "Hotel Name: $($hotel.name)"

Write-Host "`n6. Agregar habitación"
$roomBody = @{ type = "Suite"; baseCost = 350000; taxes = 63000; location = "Piso 10" } | ConvertTo-Json
$createRoomResponse = Invoke-RestMethod -Uri "$gateway_url/api/hotels/$hotel_id/rooms" -Method Post -Headers $headers -ContentType $jsonContentType -Body $roomBody
$room_id = $createRoomResponse.id
if (-not $room_id) { $room_id = $createRoomResponse.roomId }
Write-Host "Room created with ID: $room_id"

Write-Host "`n7. Habitaciones de un hotel"
$rooms = Invoke-RestMethod -Uri "$gateway_url/api/hotels/$hotel_id/rooms" -Method Get
Write-Host "Total rooms for hotel: $($rooms.Count)"

Write-Host "`n8. Actualizar habitación"
$updateRoomBody = @{ type = "Suite Presidencial"; baseCost = 500000; taxes = 90000; location = "Piso 15" } | ConvertTo-Json
Invoke-RestMethod -Uri "$gateway_url/api/hotels/$hotel_id/rooms/$room_id" -Method Put -Headers $headers -ContentType $jsonContentType -Body $updateRoomBody
Write-Host "Room updated."

Write-Host "`n9. Buscar disponibilidad"
$available = Invoke-RestMethod -Uri "$gateway_url/api/bookings/available?city=Bogot%C3%A1&arrival=2025-12-01&departure=2025-12-05&guests=2" -Method Get
Write-Host "Available hotels found: $($available.Count)"

Write-Host "`n10. Crear reserva"
$bookingBody = @{
  hotelId = $hotel_id
  roomId = $room_id
  guestFirstName = "Juan"
  guestLastName = "Pérez"
  guestDateOfBirth = "1990-05-15"
  guestGender = "M"
  guestDocumentType = "CC"
  guestDocumentNumber = "123456789"
  guestEmail = "juan.perez@email.com"
  guestPhone = "3001234567"
  arrivalDate = "2025-12-01"
  departureDate = "2025-12-05"
  numberOfGuests = 2
  emergencyContactName = "María Pérez"
  emergencyContactPhone = "3009876543"
} | ConvertTo-Json -Depth 3
$createBookingResponse = Invoke-RestMethod -Uri "$gateway_url/api/bookings" -Method Post -ContentType $jsonContentType -Body $bookingBody
$booking_id = $createBookingResponse.id
if (-not $booking_id) { $booking_id = $createBookingResponse.bookingId }
Write-Host "Booking created with ID: $booking_id"

Write-Host "`n11. Detalle de reserva"
$bookingDetail = Invoke-RestMethod -Uri "$gateway_url/api/bookings/$booking_id" -Method Get -Headers $headers
Write-Host "Booking detail fetched for guest: $($bookingDetail.guestFirstName)"

Write-Host "`n12. Reservas del hotel"
$hotelBookings = Invoke-RestMethod -Uri "$gateway_url/api/bookings/hotel/$hotel_id" -Method Get -Headers $headers
Write-Host "Total bookings for hotel: $($hotelBookings.Count)"

Write-Host "`n13. Cambiar estado habitación (Disable)"
$statusRoomBody = @{ isEnabled = $false } | ConvertTo-Json
Invoke-RestMethod -Uri "$gateway_url/api/hotels/$hotel_id/rooms/$room_id/status" -Method Patch -Headers $headers -ContentType $jsonContentType -Body $statusRoomBody
Write-Host "Room status updated."

Write-Host "`n14. Cambiar estado hotel (Disable)"
$statusHotelBody = @{ isEnabled = $false } | ConvertTo-Json
Invoke-RestMethod -Uri "$gateway_url/api/hotels/$hotel_id/status" -Method Patch -Headers $headers -ContentType $jsonContentType -Body $statusHotelBody
Write-Host "Hotel status updated."

Write-Host "`n15. Eliminar hotel (lógico)"
Invoke-RestMethod -Uri "$gateway_url/api/hotels/$hotel_id" -Method Delete -Headers $headers
Write-Host "Hotel deleted."

Write-Host "`nALL TESTS PASSED SUCCESSFULLY!"
