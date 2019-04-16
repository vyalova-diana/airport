require 'sinatra'
require 'sequel'
require 'json'

passengers = Hash.new
cargos = Hash.new

post '/passenger/:id' do |id|
  if (passangers.first(:global_id => id) == nil) then
    passangers.insert(:global_id => id)
    ""
  else 
    "{error: 1}"
  end
end

post '/cargos/:id' do |id|
  data = JSON.parse request.body.read
  data["cargos"].each { |cargo|
    cargos.insert(:passenger_id => id, :global_id => cargo["global_id"])
  }
  ""
end

get '/take/:id' do |id|
  passenger = passangers.first(:id => id)
  if (passenger != nil) then
    passenger = passangers.first(:id => id)
    passangers.where(:global_id => passenger[:global_id]).delete
    temp_cargos = cargos.where(:passenger_id => id)
    cargos_arr = []
    temp_cargos.each do |hash|
      cargo_hash = { :global_id => hash[:global_id] }
      cargos_arr << cargo_hash
    end
    temp_cargos.each { |cargo|
      cargos.where(:global_id => cargo[:global_id]).delete
    }
    cargo_json = cargos_arr.to_json
    "{id:#{id}, cargos: #{cargo_json}"
  else
    "{error: 2}"
  end
end